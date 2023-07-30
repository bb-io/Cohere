using Apps.Cohere.Models.Requests;
using Apps.Cohere.Models.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;

namespace Apps.Cohere;

[ActionList]
public class Actions
{
    private static readonly List<string> GenerateTextModels = new() 
    { 
        "base", 
        "base-light", 
        "command", 
        "command-light", 
        "command-light-nightly", 
        "command-nightly"
    };

    private static readonly List<string> EmbedModels = new()
    {
        "embed-english-light-v2.0",
        "embed-multilingual-v2.0",
        "embed-english-v2.0"
    };
    
    private static readonly List<string> TokenizeModels = new() 
    { 
        "base", 
        "base-light", 
        "command", 
        "command-light", 
        "command-light-nightly", 
        "command-nightly",
        "summarize-medium",
        "summarize-xlarge"
    };
    
    private static readonly List<string> SummarizeModels = new()
    {
        "summarize-medium",
        "summarize-xlarge"
    };
    
    private static readonly List<string> RerankModels = new()
    {
        "rerank-english-v2.0",
        "rerank-multilingual-v2.0"
    };
    
    [Action("Generate text", Description = "Generate realistic text conditioned on a given input.")]
    public async Task<GenerateTextResponse> GenerateText(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] GenerateTextRequest input)
    {
        var model = input.Model ?? "command";
        if (!GenerateTextModels.Contains(model))
            throw new Exception($"Not a valid model provided. Please provide either of: {String.Join(", ", GenerateTextModels)}");
        
        var client = new CohereClient();
        var request = new CohereRequest("/generate", Method.Post, authenticationCredentialsProviders);
        var maximumTokensNumber = input.MaximumWordsNumber * 3;
        request.AddJsonBody(new
        {
            Prompt = input.Prompt,
            Model = model,
            Max_tokens = maximumTokensNumber,
            Temperature = input.Temperature ?? 0.75,
            K = input.TopK ?? 0,
            P = input.TopP ?? 0.75,
            Frequency_penalty = input.FrequencyPenalty ?? 0.0,
            Presence_penalty = input.PresencePenalty ?? 0.0,
            Stop_sequences = input.StopSequences
        });
        
        var generations = await client.ExecuteWithHandling<GenerateTextResponseWrapper>(request);
        return generations.Generations.First();
    }
    
    [Action("Generate embedding", Description = "Generate text embedding. An embedding is a list of floating point " +
                                                "numbers that captures semantic information about the text that it " +
                                                "represents.")]
    public async Task<GenerateEmbeddingsResponse> GenerateEmbeddings(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] GenerateEmbeddingsRequest input)
    {
        var model = input.Model ?? "embed-english-v2.0";
        if (!EmbedModels.Contains(model))
            throw new Exception($"Not a valid model provided. Please provide either of: {String.Join(", ", EmbedModels)}");
        
        var client = new CohereClient();
        var request = new CohereRequest("/embed", Method.Post, authenticationCredentialsProviders);
        request.AddJsonBody(new
        {
            Texts = new[] { input.Text },
            Model = model
        });
        
        var embeddings = await client.ExecuteWithHandling<GenerateEmbeddingsResponseWrapper>(request);
        return new GenerateEmbeddingsResponse { Embedding = embeddings.Embeddings.First() };
    }
    
    [Action("Classify text", Description = "Classify text input. This action requires examples and their corresponding " +
                                           "labels to be specified. Each unique label requires at least two examples " +
                                           "associated with it.")]
    public async Task<ClassifyTextsResponse> ClassifyText(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ClassifyTextsRequest input)
    {
        if (input.ExampleTexts.Count() != input.ExampleLabels.Count())
            throw new Exception("The number of example texts should be equal to the number of example labels, so that " +
                                "each example has a corresponding label.");
        
        var model = input.Model ?? "embed-english-v2.0";
        if (!EmbedModels.Contains(model))
            throw new Exception($"Not a valid model provided. Please provide either of: {String.Join(", ", EmbedModels)}");

        var client = new CohereClient();
        var request = new CohereRequest("/classify", Method.Post, authenticationCredentialsProviders);
        request.AddJsonBody(new
        {
            Inputs = new[] { input.Text },
            Examples = input.ExampleTexts.Zip(input.ExampleLabels, (text, label) => new { text, label }),
            Model = model
        });
        
        var classifications = await client.ExecuteWithHandling<ClassifyTextsResponseWrapper>(request);
        return classifications.Classifications.First();
    }
    
    [Action("Tokenize text", Description = "Tokenize text. Specify model to ensure that the tokenization uses the " +
                                           "tokenizer used by specific model.")]
    public async Task<TokenizeTextResponse> TokenizeText(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] TokenizeTextRequest input)
    {
        var model = input.Model ?? "command";
        if (!TokenizeModels.Contains(model))
            throw new Exception($"Not a valid model provided. Please provide either of: {String.Join(", ", TokenizeModels)}");
        
        var client = new CohereClient();
        var request = new CohereRequest("/tokenize", Method.Post, authenticationCredentialsProviders);
        request.AddJsonBody(new
        {
            Text = input.Text,
            Model = model
        });
        
        var tokens = await client.ExecuteWithHandling<TokenizeTextResponse>(request);
        return tokens;
    }
    
    [Action("Detokenize tokens", Description = "Detokenize tokens. Specify model to ensure that the detokenization is " +
                                               "done by the tokenizer used by specific model.")]
    public async Task<DetokenizeTokensResponse> DetokenizeTokens(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] DetokenizeTokensRequest input)
    {
        var model = input.Model ?? "command";
        if (!TokenizeModels.Contains(model))
            throw new Exception($"Not a valid model provided. Please provide either of: {String.Join(", ", TokenizeModels)}");
        
        var client = new CohereClient();
        var request = new CohereRequest("/detokenize", Method.Post, authenticationCredentialsProviders);
        request.AddJsonBody(new
        {
            Tokens = input.Tokens,
            Model = model
        });
        
        var text = await client.ExecuteWithHandling<DetokenizeTokensResponse>(request);
        return text;
    }
    
    [Action("Detect language", Description = "Detect the language of text provided.")]
    public async Task<DetectLanguageResponse> DetectLanguage(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] DetectLanguageRequest input)
    {
        var client = new CohereClient();
        var request = new CohereRequest("/detect-language", Method.Post, authenticationCredentialsProviders);
        request.AddJsonBody(new
        {
            Texts = new[] { input.Text }
        });
        
        var detection = await client.ExecuteWithHandling<DetectLanguageResponseWrapper>(request);
        return detection.Results.First();
    }
    
    [Action("Summarize text", Description = "Summarize the text provided.")]
    public async Task<SummarizeTextResponse> SummarizeText(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] SummarizeTextRequest input)
    {
        var model = input.Model ?? "summarize-xlarge";
        if (!SummarizeModels.Contains(model))
            throw new Exception($"Not a valid model provided. Please provide either of: {String.Join(", ", SummarizeModels)}");
        
        var client = new CohereClient();
        var request = new CohereRequest("/summarize", Method.Post, authenticationCredentialsProviders);
        request.AddJsonBody(new
        {
            Text = input.Text,
            Length = input.Length,
            Format = input.Format,
            Model = model,
            Extractiveness = input.Extractiveness,
            Temperature = input.Temperature ?? 0.75,
            Additional_command = input.AdditionalCommand
        });
        
        var summary = await client.ExecuteWithHandling<SummarizeTextResponse>(request);
        return summary;
    }
    
    [Action("Rerank texts", Description = "This action takes in a query and a list of texts and produces an ordered " +
                                          "list with each text assigned a relevance score.")]
    public async Task<RerankTextsResponse> RerankTexts(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] RerankTextsRequest input)
    {
        var model = input.Model ?? "rerank-multilingual-v2.0";
        if (!RerankModels.Contains(model))
            throw new Exception($"Not a valid model provided. Please provide either of: {String.Join(", ", RerankModels)}");

        if (input.MinimumRelevanceScore != null && (input.MinimumRelevanceScore < 0 || input.MinimumRelevanceScore > 1))
            throw new Exception("Value of minimum relevance score parameter must be in range from 0.0 to 1.0");
        
        var client = new CohereClient();
        var request = new CohereRequest("/rerank", Method.Post, authenticationCredentialsProviders);
        request.AddJsonBody(new
        {
            Query = input.Query,
            Documents = input.Texts,
            Model = model,
            Return_documents = true
        });
        
        var rerankedTexts = await client.ExecuteWithHandling<RerankTextsResponse>(request);

        if (input.MinimumRelevanceScore != null)
            rerankedTexts.Results = rerankedTexts.Results.Where(t => t.RelevanceScore >= input.MinimumRelevanceScore);

        return rerankedTexts;
    }
}