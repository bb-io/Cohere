using Apps.Cohere.DataSourceHandlers;
using Apps.Cohere.Dtos;
using Apps.Cohere.Models.Requests;
using Apps.Cohere.Models.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Filters.Constants;
using Blackbird.Filters.Enums;
using Blackbird.Filters.Extensions;
using Blackbird.Filters.Transformations;
using MathNet.Numerics.LinearAlgebra;
using RestSharp;

namespace Apps.Cohere.Actions;

[ActionList("Chat")]
public class ChatActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
    : Invocable(invocationContext)
{
    private Task<string> ResolveChatModelAsync(string? model, CancellationToken cancellationToken = default)
        => string.IsNullOrWhiteSpace(model)
            ? Client.ResolveDefaultModelAsync(CohereModelEndpoints.Chat, cancellationToken)
            : Task.FromResult(model);

    private Task<string> ResolveTokenizeModelAsync(string? model, CancellationToken cancellationToken = default)
        => string.IsNullOrWhiteSpace(model)
            ? Client.ResolveDefaultModelAsync(CohereModelEndpoints.Chat, cancellationToken)
            : Task.FromResult(model);

    [Action("Generate text", Description = "Generate realistic text conditioned on a given input.")]
    public async Task<GenerateTextResponse> GenerateText([ActionParameter] GenerateTextRequest input)
    {
        var model = await ResolveChatModelAsync(input.Model);
        var request = new CohereRequest("/chat", Method.Post, Creds);

        request.AddJsonBody(new
        {
            message = input.Prompt,
            model,
            max_tokens = input.MaximumTokensNumber > 0 ? input.MaximumTokensNumber : 100,
            temperature = input.Temperature ?? 0.0f,
            k = input.TopK ?? 0,
            p = input.TopP ?? 1.0f,
            stop_sequences = input.StopSequences
        });

        var response = await Client.ExecuteWithErrorHandling<GenerateTextResponse>(request);
        return new GenerateTextResponse { Text = response.Text ?? string.Empty };
    }

    [Action("Extract entity from text", Description = "Extract a piece of information from text. Provide entity that " +
                                                      "you want to extract from a text (e.g. product title).")]
    public async Task<ExtractEntityFromTextResponse> ExtractEntityFromText([ActionParameter] ExtractEntityFromTextRequest input)
    {
        var model = await ResolveChatModelAsync(input.Model);

        var request = new CohereRequest("/chat", Method.Post, Creds);

        var prompt = $"Extract the {input.Entity} from the following text. " +
                     $"Return only the value with no extra words or labels.\n\nText:\n{input.Text}";

        request.AddJsonBody(new
        {
            message = prompt,
            model,
            max_tokens = input.MaximumTokensNumber ?? 100,
            temperature = input.Temperature ?? 0.0f,
        });

        var resp = await Client.ExecuteWithErrorHandling<ExtractEntityFromTextResponse>(request);

        return new ExtractEntityFromTextResponse
        {
            Text = resp?.Text?.Trim() ?? string.Empty
        };
    }

    [Action("Edit text", Description = "Edit the input text given an instruction prompt (e.g. make it more concise or " +
                                      "make it more friendly).")]
    public async Task<EditTextResponse> EditText([ActionParameter] EditTextRequest input)
    {
        var model = await ResolveChatModelAsync(input.Model);
        var request = new CohereRequest("/chat", Method.Post, Creds);

        var prompt = $"Edit the following text to {input.Instruction} : {input.Text}";

        float? frequencyPenalty = input.FrequencyPenalty;
        float? presencePenalty = input.PresencePenalty;
        if (frequencyPenalty.HasValue && presencePenalty.HasValue)
            presencePenalty = null;

        request.AddJsonBody(new
        {
            message = prompt,
            model,
            max_tokens = input.MaximumTokensNumber,
            temperature = input.Temperature ?? 0.3f,
            k = input.TopK ?? 0,
            p = input.TopP ?? 0.75f,
            frequency_penalty = frequencyPenalty,
            presence_penalty = presencePenalty,
            stop_sequences = input.StopSequences
        });

        var response = await Client.ExecuteWithErrorHandling<EditTextResponse>(request);
        return new EditTextResponse { Text = response.Text ?? string.Empty };
    }

    [Action("Perform grammar and spelling check", Description = "Perform a grammar and spelling check of the text provided.")]
    public async Task<PerformGrammarAndSpellingCheckResponse> PerformGrammarAndSpellingCheck([ActionParameter] PerformGrammarAndSpellingCheckRequest input)
    {
        async Task<int> GetTokensNumber(CohereClient client, string text, string model)
        {
            var request = new CohereRequest("/tokenize", Method.Post, Creds);
            request.AddJsonBody(new
            {
                Text = text,
                Model = model
            });
            var tokens = await client.ExecuteWithErrorHandling<TokensDto>(request);
            return tokens.Tokens.Length;
        }

        var model = await ResolveChatModelAsync(input.Model);

        var maximumTokensNumber = await GetTokensNumber(Client, input.Text, model) + 20;
        var request = new CohereRequest("/chat", Method.Post, Creds);
        request.AddJsonBody(new
        {
            message =
                $"Proofread the following text for grammar and spelling. " +
                $"Return only the corrected text with no explanations.\n\nText:\n{input.Text}",
            model,
            max_tokens = maximumTokensNumber,
            temperature = 0.0f
        });

        var response = await Client.ExecuteWithErrorHandling<PerformGrammarAndSpellingCheckResponse>(request);
        return new PerformGrammarAndSpellingCheckResponse
        {
            Text = response.Text ?? string.Empty
        };
    }

    [Action("Analyze text", Description = "Analyze text to retrieve information about its style, mood and tone.")]
    public async Task<AnalyzeTextResponse> AnalyzeText([ActionParameter] AnalyzeTextRequest input)
    {
        var model = await ResolveChatModelAsync(input.Model);
        var prompt = @$"
                This is a few words description of style, mood and tone generator.

                Text: We can solve the problem of applications operating in isolation and in dispersed modalities by 
                enabling integration, automation, and seamless analytics between the apps, data, content, participants, 
                and devices used for localization and globalization management. However, Blackbird goes beyond connecting 
                applications, data, and content within an organization. It can also connect organizations into networks 
                - e.g. the end clients who create the content with multiple localization vendors. Our mission is to 
                become the global language services industry’s best iPaaS and automation-platform-as-a-service technology 
                vendor. At Blackbird, our integration platform revolves around Birds. All the time. Lines of code are 
                transformed into the four forces of flight: lift (triggers), weight (actions), thrust (apps), and 
                drag (connections). The way the four forces act on your Bird makes it do different things. Fine, but how, 
                you could ask. Think about the apps you use for your business. Your initial list should have at least 
                one e-mail client, project management software, translation memory software or billing software. You can 
                connect and make them communicate with each other in Blackbird.
                
                Result: Professional and informative style, positive and excited mood, educative tone.
                
                Text: {input.Text}
                
                Result:
            ";

        var request = new CohereRequest("/chat", Method.Post, Creds);

        request.AddJsonBody(new
        {
            message = prompt,
            model,
            max_tokens = 100,
            temperature = 0.0f
        });

        var response = await Client.ExecuteWithErrorHandling<AnalyzeTextResponse>(request);
        return new AnalyzeTextResponse
        {
            Text = response.Text ?? string.Empty
        };
    }

    [Action("Summarise text analyses", Description =
        "Summarise information about styles, moods and tones of different " +
        "texts to find common patterns in styles, moods and tones.")]
    public async Task<SummariseTextAnalysesResponse> SummariseTextAnalyses([ActionParameter] SummariseTextAnalysesRequest input)
    {
        var model = await ResolveChatModelAsync(input.Model);
        var analyses = input.TextAnalyses != null ? string.Join("\n", input.TextAnalyses) : string.Empty;
        var prompt = @$"
                This is a common patterns in styles, moods and tones analyser.

                Text:
                Scientific and informative style, objective and detached mood, expository tone.
                Professional and informative style, informative and historical mood, informative tone.
                Informative and descriptive style, positive and uplifting mood, informative tone.
                Factual and informative style, neutral and descriptive mood, informative tone.
                Professional and informative style, calm and confident mood, informative tone.
                Informative and educational style, positive and excited mood, expository tone.

                Result: Informative and professional style,  objective, neutral, and informative mood, informative and educational tone.

                Text:
                {analyses}

                Result:
                ";

        var request = new CohereRequest("/chat", Method.Post, Creds);

        request.AddJsonBody(new
        {
            message = prompt,
            model,
            max_tokens = 150,
            temperature = 0.1f
        });

        var response = await Client.ExecuteWithErrorHandling<SummariseTextAnalysesResponse>(request);
        return new SummariseTextAnalysesResponse
        {
            Text = response.Text ?? string.Empty
        };
    }

    [Action("Reshape text",
        Description = "Reshape the text. Provide the information about target style, mood and tone.")]
    public async Task<ReshapeTextResponse> ReshapeText([ActionParameter] ReshapeTextRequest input)
    {
        var model = await ResolveChatModelAsync(input.Model);
        var additionalInstruction = input.AdditionalInstruction ?? "";
        var prompt = @$"
                This is a rewriter of the input text which reshapes the text so that it matches the target style, mood, and tone.

                Text:
                We can solve the problem of applications operating in isolation and in dispersed modalities by enabling integration, 
                automation, and seamless analytics between the apps, data, content, participants, and devices used for localization 
                and globalization management. However, Blackbird goes beyond connecting applications, data, and content within an 
                organization. It can also connect organizations into networks - e.g. the end clients who create the content with 
                multiple localization vendors. Our mission is to become the global language services industry’s best iPaaS and 
                automation-platform-as-a-service technology vendor. At Blackbird, our integration platform revolves around Birds. 
                All the time. Lines of code are transformed into the four forces of flight: lift (triggers), weight (actions), 
                thrust (apps), and drag (connections). The way the four forces act on your Bird makes it do different things. Fine, 
                but how, you could ask. Think about the apps you use for your business. Your initial list should have at least 
                one e-mail client, project management software, translation memory software or billing software. You can connect 
                and make them communicate with each other in Blackbird.

                Target style, mood, and tone: 
                Enchanting, magical, adventurous, thrilling, suspenseful, mysterious, and exciting style, positive and uplifting 
                mood, engaging and immersive tone.

                Additional instruction: 

                Result:
                Step into the world of Blackbird, where enchanting possibilities await. Our magical platform holds the key to 
                adventurous solutions for applications, dispersed modalities, and seamless analytics. Let the thrill of integration, 
                automation, and connectivity sweep you off your feet as you embark on a suspenseful journey.
                At Blackbird, we go beyond ordinary connections within organizations. We create networks that span across end 
                clients, content creators, and multiple localization vendors. Our mission is to become the beacon of the global 
                language services industry, the best iPaaS, and automation-platform-as-a-service technology vendor.
                Imagine the immersive experience of our integration platform, where lines of code transform into the four forces 
                of flight - lift, weight, thrust, and drag. Like the majestic wings of a bird, your applications, data, and 
                content come together in harmony. Your business apps, the e-mail client, project management software, translation 
                memory software, or billing software, they all converge and communicate within the captivating world of Blackbird.
                With positive energy and uplifting spirit, we invite you to soar with us, discovering the endless possibilities 
                of our enchanted platform. The mysteries of technology unfold before your eyes, and excitement fills the air. 
                Welcome to the world of Blackbird, where magic and innovation intertwine, and your dreams take flight.

                Text: 
                {input.Text}                

                Target style, mood, and tone: 
                {input.ReshapeInstructions}

                Additional instruction: 
                {additionalInstruction}

                Result:  
                ";

        var request = new CohereRequest("/chat", Method.Post, Creds);

        request.AddJsonBody(new
        {
            message = prompt,
            model,
            max_tokens = input.MaximumTokensNumber > 0 ? input.MaximumTokensNumber : 300,
            temperature = input.Temperature ?? 1.0f
        });

        var response = await Client.ExecuteWithErrorHandling<ReshapeTextResponse>(request);
        return new ReshapeTextResponse
        {
            Text = response.Text ?? string.Empty
        };
    }

    [Action("Detect locale", Description = "Detect locale of the text provided.")]
    public async Task<DetectLocaleResponse> DetectLocale([ActionParameter] DetectLocaleRequest input)
    {
        var model = await ResolveChatModelAsync(input.Model);
        var prompt = @$"
                This is a locale detector.

                Text: Як працюють великі мовні моделі?
                Locale: uk_UA

                Text: Esta canción está poca madre.
                Locale: es-MX

                Text: Do you wanna hang out later this avo?
                Locale: en_AU

                Text: 你想待会儿一起出去吗
                Locale: zh_CN 

                Text: Londres est la capitale de la Grande-Bretagne
                Locale: fr_FR

                Text: {input.Text}
                Locale:
                ";

        var request = new CohereRequest("/chat", Method.Post, Creds);
        request.AddJsonBody(new
        {
            message = prompt,
            model,
            max_tokens = 16,
            temperature = 0.0f,

        });

        var response = await Client.ExecuteWithErrorHandling<DetectLocaleResponse>(request);
        return new DetectLocaleResponse
        {
            Text = response.Text ?? string.Empty
        };
    }

    [Action("Calculate similarity of two texts", Description =
        "Calculate the similarity of texts provided. The result " +
        "of this action is a percentage similarity score. The " +
        "higher the score, the more similar the texts are.")]
    public async Task<CalculateTextsSimilarityResponse> CalculateTextsSimilarity([ActionParameter] CalculateTextsSimilarityRequest input)
    {
        double CalculateSimilarityScore(Vector<double> firstTextEmbedding, Vector<double> secondTextEmbedding)
        {
            var embeddingsDotProduct = firstTextEmbedding.DotProduct(secondTextEmbedding);
            var firstTextEmbeddingNorm = firstTextEmbedding.L2Norm();
            var secondTextEmbeddingNorm = secondTextEmbedding.L2Norm();
            var similarityScore = embeddingsDotProduct / (firstTextEmbeddingNorm * secondTextEmbeddingNorm);
            return similarityScore;
        }

        var model = input.Model ?? "embed-english-v3.0";
        var request = new CohereRequest("/embed", Method.Post, Creds);
        request.AddJsonBody(new
        {
            texts = new[] { input.FirstText, input.SecondText },
            model,
            input_type = "search_document"
        });

        var embeddings = await Client.ExecuteWithErrorHandling<EmbeddingsDto>(request);
        var firstTextEmbedding = Vector<double>.Build.DenseOfArray(embeddings.Embeddings[0]);
        var secondTextEmbedding = Vector<double>.Build.DenseOfArray(embeddings.Embeddings[1]);
        var similarityScore = CalculateSimilarityScore(firstTextEmbedding, secondTextEmbedding);
        var similarityScoreInPercents = Math.Round((decimal)similarityScore * 100, 2);
        return new CalculateTextsSimilarityResponse { SimilarityScore = similarityScoreInPercents };
    }

    [Action("Rerank texts", Description = "This action takes in a query and a list of texts and produces an ordered " +
                                          "list with each text assigned a relevance score.")]
    public async Task<RerankTextsResponse> RerankTexts([ActionParameter] RerankTextsRequest input)
    {
        var model = input.Model ?? "rerank-multilingual-v3.0";
        var request = new CohereRequest("/rerank", Method.Post, Creds);
        request.AddJsonBody(new
        {
            query = input.Query,
            documents = input.Texts,
            model,
            top_n = input.TopN ?? input.Texts.Count(),
            return_documents = true
        });

        var rerankedTexts = await Client.ExecuteWithErrorHandling<RerankedTextDtoWrapper>(request);

        if (input.MinimumRelevanceScore != null)
            rerankedTexts.Results = rerankedTexts.Results.Where(t => t.RelevanceScore >= input.MinimumRelevanceScore);

        var resultText = string.Join("\n", rerankedTexts.Results.Select(r => r.Text));
        return new RerankTextsResponse { RerankedTexts = resultText };
    }

    [Action("Rerank texts provided in a file", Description = "This action takes in a query and a txt file with list " +
                                                             "of texts and produces a text combined from most relevant " +
                                                             "texts. Each text in the file must start on a new line.")]
    public async Task<RerankTextsResponse> RerankTextsProvidedInFile([ActionParameter] RerankTextsProvidedInFileRequest input)
    {
        async Task<List<string>> GetDocumentsFromFile(FileReference file)
        {
            var documents = new List<string>();

            await using var stream = await fileManagementClient.DownloadAsync(file);
            using var reader = new StreamReader(stream);
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                documents.Add(line);
            }

            return documents;
        }

        var model = input.Model ?? "rerank-multilingual-v3.0";
        var fileExtension = input.TxtFileWithTexts.Name.Split(".")[^1];
        if (fileExtension != "txt")
            throw new PluginMisconfigurationException("Please provide txt file");

        var request = new CohereRequest("/rerank", Method.Post, Creds);
        var documents = await GetDocumentsFromFile(input.TxtFileWithTexts);
        if (documents.Count == 0)
            throw new PluginMisconfigurationException("The provided txt file has no valid lines.");

        var topN = input.TopN > 0 ? input.TopN : documents.Count;

        request.AddJsonBody(new
        {
            query = input.Query,
            documents,
            model,
            top_n = topN,
            return_documents = true
        });

        var rerankedTexts = await Client.ExecuteWithErrorHandling<RerankedTextDtoWrapper>(request);

        if (input.MinimumRelevanceScore != null)
            rerankedTexts.Results = rerankedTexts.Results.Where(t => t.RelevanceScore >= input.MinimumRelevanceScore);

        var resultText = string.Join("\n", rerankedTexts.Results.Select(r => r.Text));
        return new RerankTextsResponse { RerankedTexts = resultText };
    }

    [Action("Generate embedding", Description = "Generate text embedding. An embedding is a list of floating point " +
                                                "numbers that captures semantic information about the text that it " +
                                                "represents.")]
    public async Task<GenerateEmbeddingResponse> GenerateEmbedding([ActionParameter] GenerateEmbeddingRequest input)
    {
        var model = input.Model ?? "embed-english-v3.0";
        var request = new CohereRequest("/embed", Method.Post, Creds);
        request.AddJsonBody(new
        {
            texts = new[] { input.Text },
            model,
        });

        var embeddings = await Client.ExecuteWithErrorHandling<GenerateEmbeddingResponseWrapper>(request);
        return new GenerateEmbeddingResponse { Embedding = embeddings.Embeddings.First() };
    }

    [Action("Tokenize text", Description = "Tokenize text. Specify model to ensure that the tokenization uses the " +
                                           "tokenizer used by specific model.")]
    public async Task<TokenizeTextResponse> TokenizeText([ActionParameter] TokenizeTextRequest input)
    {
        var model = await ResolveTokenizeModelAsync(input.Model);
        var request = new CohereRequest("/tokenize", Method.Post, Creds);
        request.AddJsonBody(new
        {
            text = input.Text,
            model
        });

        var tokens = await Client.ExecuteWithErrorHandling<TokenizeTextResponse>(request);
        return tokens;
    }
}
