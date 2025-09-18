using System.Globalization;
using Apps.Cohere.Dtos;
using Apps.Cohere.Models.Requests;
using Apps.Cohere.Models.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;
using MathNet.Numerics.LinearAlgebra;
using RestSharp;

namespace Apps.Cohere;

[ActionList]
public class Actions : Invocable
{
    private readonly IFileManagementClient _fileManagementClient;

    public Actions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : base(
        invocationContext)
    {
        _fileManagementClient = fileManagementClient;
    }

    [Action("Generate text", Description = "Generate realistic text conditioned on a given input.")]
    public async Task<GenerateTextResponse> GenerateText([ActionParameter] GenerateTextRequest input)
    {
        var model = input.Model ?? "command";
        var request = new CohereRequest("/chat", Method.Post,Creds);

        request.AddJsonBody(new
        {
            message = input.Prompt,
            model = model,
            max_tokens = input.MaximumTokensNumber > 0 ? input.MaximumTokensNumber : 8,
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
    public async Task<ExtractEntityFromTextResponse> ExtractEntityFromText(
        [ActionParameter] ExtractEntityFromTextRequest input)
    {
        var model = string.IsNullOrWhiteSpace(input.Model) ? "command-a-03-2025" : input.Model;

        var request = new CohereRequest("/chat", Method.Post, Creds);

        var prompt = $"Extract the {input.Entity} from the following text. " +
                     $"Return only the value with no extra words or labels.\n\nText:\n{input.Text}";

        request.AddJsonBody(new
        {
            message = prompt,
            model = model,
            max_tokens = (input.MaximumTokensNumber ?? 100),
            temperature = input.Temperature ?? 0.0f,
        });

        var resp = await Client.ExecuteWithErrorHandling<ExtractEntityFromTextResponse>(request);

        return new ExtractEntityFromTextResponse
        {
            Text = resp?.Text?.Trim() ?? string.Empty
        };
    }

    [Action("Edit text", Description ="Edit the input text given an instruction prompt (e.g. make it more concise or " +
                                      "make it more friendly).")]
    public async Task<EditTextResponse> EditText([ActionParameter] EditTextRequest input)
    {
        var model = input.Model ?? "command-a-03-2025";
        var request = new CohereRequest("/chat", Method.Post, Creds);

        var prompt = $"Edit the following text to {input.Instruction} : {input.Text}";

        float? frequencyPenalty = input.FrequencyPenalty;
        float? presencePenalty = input.PresencePenalty;
        if (frequencyPenalty.HasValue && presencePenalty.HasValue)
            presencePenalty = null;

        request.AddJsonBody(new
        {
            message = prompt,
            model = model,
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

    [Action("Perform grammar and spelling check",Description = "Perform a grammar and spelling check of the text provided.")]
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

        var model = input.Model ?? "command-a-03-2025";

        var maximumTokensNumber = await GetTokensNumber(Client, input.Text, model) + 20;
        var request = new CohereRequest("/chat", Method.Post, Creds);
        request.AddJsonBody(new
        {
            message =
                $"Proofread the following text for grammar and spelling. " +
                $"Return only the corrected text with no explanations.\n\nText:\n{input.Text}",
            model = model,
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
        var model = input.Model ?? "command-a-03-2025";
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
            model = model,
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
        var model = input.Model ?? "command-a-03-2025";
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
            model = model,
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
    public async Task<ReshapeTextResponse> ReshapeText(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ReshapeTextRequest input)
    {
        var model = input.Model ?? "command";
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

        var client = new CohereClient();
        var request = new CohereRequest("/generate", Method.Post, authenticationCredentialsProviders);
        request.AddJsonBody(new
        {
            Prompt = prompt,
            Model = model,
            Max_tokens = input.MaximumTokensNumber,
            Temperature = input.Temperature ?? 1
        });

        var generations = await client.ExecuteWithErrorHandling<ReshapeTextResponseWrapper>(request);
        return generations.Generations.First();
    }

    [Action("Detect locale", Description = "Detect locale of the text provided.")]
    public async Task<DetectLocaleResponse> DetectLocale(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] DetectLocaleRequest input)
    {
        var model = input.Model ?? "command";
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

        var client = new CohereClient();
        var request = new CohereRequest("/generate", Method.Post, authenticationCredentialsProviders);
        request.AddJsonBody(new
        {
            Prompt = prompt,
            Model = model,
            Temperature = 0
        });

        var generations = await client.ExecuteWithErrorHandling<DetectLocaleResponseWrapper>(request);
        return generations.Generations.First();
    }

    [Action("Calculate similarity of two texts", Description =
        "Calculate the similarity of texts provided. The result " +
        "of this action is a percentage similarity score. The " +
        "higher the score, the more similar the texts are.")]
    public async Task<CalculateTextsSimilarityResponse> CalculateTextsSimilarity(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] CalculateTextsSimilarityRequest input)
    {
        double CalculateSimilarityScore(Vector<double> firstTextEmbedding, Vector<double> secondTextEmbedding)
        {
            var embeddingsDotProduct = firstTextEmbedding.DotProduct(secondTextEmbedding);
            var firstTextEmbeddingNorm = firstTextEmbedding.L2Norm();
            var secondTextEmbeddingNorm = secondTextEmbedding.L2Norm();
            var similarityScore = embeddingsDotProduct / (firstTextEmbeddingNorm * secondTextEmbeddingNorm);
            return similarityScore;
        }

        var model = input.Model ?? "embed-english-v2.0";
        var client = new CohereClient();
        var request = new CohereRequest("/embed", Method.Post, authenticationCredentialsProviders);
        request.AddJsonBody(new
        {
            Texts = new[] { input.FirstText, input.SecondText },
            Model = model
        });

        var embeddings = await client.ExecuteWithErrorHandling<EmbeddingsDto>(request);
        var firstTextEmbedding = Vector<double>.Build.DenseOfArray(embeddings.Embeddings[0]);
        var secondTextEmbedding = Vector<double>.Build.DenseOfArray(embeddings.Embeddings[1]);
        var similarityScore = CalculateSimilarityScore(firstTextEmbedding, secondTextEmbedding);
        var similarityScoreInPercents = Math.Round((decimal)similarityScore * 100, 2);
        return new CalculateTextsSimilarityResponse { SimilarityScore = similarityScoreInPercents };
    }

    [Action("Classify text", Description =
        "Classify text input. This action requires examples and their corresponding " +
        "labels to be specified. Each unique label requires at least two examples " +
        "associated with it.")]
    public async Task<ClassifyTextsResponse> ClassifyText(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ClassifyTextsRequest input)
    {
        if (input.ExampleTexts.Count() != input.ExampleLabels.Count())
            throw new Exception(
                "The number of example texts should be equal to the number of example labels, so that " +
                "each example has a corresponding label.");

        var model = input.Model ?? "embed-english-v2.0";
        var client = new CohereClient();
        var request = new CohereRequest("/classify", Method.Post, authenticationCredentialsProviders);
        request.AddJsonBody(new
        {
            Inputs = new[] { input.Text },
            Examples = input.ExampleTexts.Zip(input.ExampleLabels, (text, label) => new { text, label }),
            Model = model
        });

        var classifications = await client.ExecuteWithErrorHandling<ClassifyTextsResponseWrapper>(request);
        return classifications.Classifications.First();
    }

    [Action("Classify text with examples as a file", Description = "Classify text input. This action requires a csv " +
                                                                   "file with examples and their corresponding labels. " +
                                                                   "Each file's line should have the form 'example, " +
                                                                   "label'. Each unique label requires at least two " +
                                                                   "examples associated with it.")]
    public async Task<ClassifyTextsResponse> ClassifyTextWithFileExamples(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ClassifyTextWithFileExamplesRequest input)
    {
        async Task<IEnumerable<ClassificationExampleDto>> GetExamplesFromCsvFile(FileReference csvFile)
        {
            await using var stream = await _fileManagementClient.DownloadAsync(csvFile);
            using var reader = new StreamReader(stream);
            using var csvReader = new CsvReader(reader,
                new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false });
            var examples = csvReader.GetRecords<ClassificationExampleDto>().ToList();
            return examples;
        }

        var model = input.Model ?? "embed-english-v2.0";
        var fileExtension = input.CsvFileWithExamples.Name.Split(".")[^1];
        if (fileExtension != "csv")
            throw new Exception("Please provide csv file");

        var client = new CohereClient();
        var request = new CohereRequest("/classify", Method.Post, authenticationCredentialsProviders);
        request.AddJsonBody(new
        {
            Inputs = new[] { input.Text },
            Examples = (await GetExamplesFromCsvFile(input.CsvFileWithExamples))
                .Select(item => new { text = item.Text, label = item.Label }),
            Model = model
        });

        var classifications = await client.ExecuteWithErrorHandling<ClassifyTextsResponseWrapper>(request);
        return classifications.Classifications.First();
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

        var detection = await client.ExecuteWithErrorHandling<DetectLanguageResponseWrapper>(request);
        return detection.Results.First();
    }

    [Action("Summarize text", Description = "Summarize the text provided.")]
    public async Task<SummarizeTextResponse> SummarizeText(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] SummarizeTextRequest input)
    {
        var model = input.Model ?? "summarize-xlarge";
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

        var summary = await client.ExecuteWithErrorHandling<SummarizeTextResponse>(request);
        return summary;
    }

    [Action("Rerank texts", Description = "This action takes in a query and a list of texts and produces an ordered " +
                                          "list with each text assigned a relevance score.")]
    public async Task<RerankTextsResponse> RerankTexts(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] RerankTextsRequest input)
    {
        var model = input.Model ?? "rerank-multilingual-v2.0";
        var client = new CohereClient();
        var request = new CohereRequest("/rerank", Method.Post, authenticationCredentialsProviders);
        request.AddJsonBody(new
        {
            Query = input.Query,
            Documents = input.Texts,
            Model = model,
            Top_n = input.TopN ?? input.Texts.Count(),
            Return_documents = true
        });

        var rerankedTexts = await client.ExecuteWithErrorHandling<RerankedTextDtoWrapper>(request);

        if (input.MinimumRelevanceScore != null)
            rerankedTexts.Results = rerankedTexts.Results.Where(t => t.RelevanceScore >= input.MinimumRelevanceScore);

        var resultText = string.Join("\n", rerankedTexts.Results.Select(r => r.Text));
        return new RerankTextsResponse { RerankedTexts = resultText };
    }

    [Action("Rerank texts provided in a file", Description = "This action takes in a query and a txt file with list " +
                                                             "of texts and produces a text combined from most relevant " +
                                                             "texts. Each text in the file must start on a new line.")]
    public async Task<RerankTextsResponse> RerankTextsProvidedInFile(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] RerankTextsProvidedInFileRequest input)
    {
        async Task<List<string>> GetDocumentsFromFile(FileReference file)
        {
            var documents = new List<string>();

            await using var stream = await _fileManagementClient.DownloadAsync(file);
            using var reader = new StreamReader(stream);
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                documents.Add(line);
            }

            return documents;
        }

        var model = input.Model ?? "rerank-multilingual-v2.0";
        var fileExtension = input.TxtFileWithTexts.Name.Split(".")[^1];
        if (fileExtension != "txt")
            throw new Exception("Please provide txt file");

        var client = new CohereClient();
        var request = new CohereRequest("/rerank", Method.Post, authenticationCredentialsProviders);
        var documents = await GetDocumentsFromFile(input.TxtFileWithTexts);
        request.AddJsonBody(new
        {
            Query = input.Query,
            Documents = documents,
            Model = model,
            Top_n = input.TopN,
            Return_documents = true
        });

        var rerankedTexts = await client.ExecuteWithErrorHandling<RerankedTextDtoWrapper>(request);

        if (input.MinimumRelevanceScore != null)
            rerankedTexts.Results = rerankedTexts.Results.Where(t => t.RelevanceScore >= input.MinimumRelevanceScore);

        var resultText = string.Join("\n", rerankedTexts.Results.Select(r => r.Text));
        return new RerankTextsResponse { RerankedTexts = resultText };
    }

    [Action("Generate embedding", Description = "Generate text embedding. An embedding is a list of floating point " +
                                                "numbers that captures semantic information about the text that it " +
                                                "represents.")]
    public async Task<GenerateEmbeddingResponse> GenerateEmbedding(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] GenerateEmbeddingRequest input)
    {
        var model = input.Model ?? "embed-english-v2.0";
        var client = new CohereClient();
        var request = new CohereRequest("/embed", Method.Post, authenticationCredentialsProviders);
        request.AddJsonBody(new
        {
            Texts = new[] { input.Text },
            Model = model
        });

        var embeddings = await client.ExecuteWithErrorHandling<GenerateEmbeddingResponseWrapper>(request);
        return new GenerateEmbeddingResponse { Embedding = embeddings.Embeddings.First() };
    }

    [Action("Tokenize text", Description = "Tokenize text. Specify model to ensure that the tokenization uses the " +
                                           "tokenizer used by specific model.")]
    public async Task<TokenizeTextResponse> TokenizeText(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] TokenizeTextRequest input)
    {
        var model = input.Model ?? "command";
        var client = new CohereClient();
        var request = new CohereRequest("/tokenize", Method.Post, authenticationCredentialsProviders);
        request.AddJsonBody(new
        {
            Text = input.Text,
            Model = model
        });

        var tokens = await client.ExecuteWithErrorHandling<TokenizeTextResponse>(request);
        return tokens;
    }
}