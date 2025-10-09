using Apps.Cohere;
using Apps.Cohere.Actions;
using Apps.Cohere.Models.Requests;
using Newtonsoft.Json;
using System.Text;
using Tests.Cohere.Base;

namespace Tests.Cohere
{
    [TestClass]
    public class ActionTest : TestBase
    {
        [TestMethod]
        public async Task GenerateText_IssSuccess()
        {
            var action = new ChatActions(InvocationContext, FileManager);

            var result = await action.GenerateText(new Apps.Cohere.Models.Requests.GenerateTextRequest
            {
                Prompt = "Reply with a single word: OK",
                MaximumTokensNumber = 8,
                Temperature = 0.0f,
                TopK = 0,
                TopP = 1.0f,
                StopSequences = new[] { "\n" },
                Model = "command-a-03-2025"
            });

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
            Console.WriteLine(json);

            Assert.IsNotNull(result);
        }


        [TestMethod]
        public async Task ExtractEntityFromText_ShouldReturnValue()
        {
            var action = new ChatActions(InvocationContext, FileManager);

            var input = new ExtractEntityFromTextRequest
            {
                Text = "Order #1001 details: Product title is Cozy Blanket. Price: $49.99.",
                Entity = "product title",
                MaximumTokensNumber = 12,
                Temperature = 0.0f,
                Model = "command-a-03-2025"
            };
            var result = await action.ExtractEntityFromText(input);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task EditText_ShouldReturnEditedText()
        {
            var action = new ChatActions(InvocationContext, FileManager);

            var input = new EditTextRequest
            {
                Text = "hi victor, we checked the issue pls fix asap. thx!",
                Instruction = "make it more formal and polite",
                MaximumTokensNumber = 100,
                Temperature = 0.0f,
                TopK = 0,
                TopP = 1.0f,
                StopSequences = new[] { "\n\n" },
                Model = "command-a-03-2025"
            };

            var result = await action.EditText(input);

            var json = JsonConvert.SerializeObject(result, Formatting.Indented);
            Console.WriteLine(json);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task PerformGrammarAndSpellingCheck_ShouldReturnCorrectedText()
        {
            var action = new ChatActions(InvocationContext, FileManager);

            var input = new PerformGrammarAndSpellingCheckRequest
            {
                Text = "we was going to the store yesterday but it dont open.",
                Model = "command-a-03-2025"
            };

            var result = await action.PerformGrammarAndSpellingCheck(input);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task AnalyzeText_ShouldReturnAnalysis()
        {
            var action = new ChatActions(InvocationContext, FileManager);

            var input = new AnalyzeTextRequest
            {
                Text = "Our new feature speeds up project setup and makes onboarding delightful for teams of all sizes.",
                Model = "command-a-03-2025"
            };

            var result = await action.AnalyzeText(input);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task SummariseTextAnalyses_ShouldReturnSummary()
        {
            var action = new ChatActions(InvocationContext, FileManager);

            var input = new SummariseTextAnalysesRequest
            {
                TextAnalyses = new[]
                {
                "Professional and informative style, positive and excited mood, educative tone.",
                "Informative and descriptive style, neutral mood, expository tone.",
                "Professional and concise style, calm mood, informative tone."
            },
                Model = "command-a-03-2025"
            };

            var result = await action.SummariseTextAnalyses(input);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ReshapeText_ShouldReturnReshapedText()
        {
            var action = new ChatActions(InvocationContext, FileManager);

            var input = new ReshapeTextRequest
            {
                Text = "Our platform connects apps and automates workflows to reduce manual effort and improve delivery speed.",
                ReshapeInstructions = "professional, concise style; confident mood; informative tone",
                MaximumTokensNumber = 120,
                Temperature = 0.3f,
                Model = "command-a-03-2025"
            };

            var result = await action.ReshapeText(input);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task DetectLocale_ShouldReturnLocale()
        {
            var action = new ChatActions(InvocationContext, FileManager);

            var input = new DetectLocaleRequest
            {
                Text = "Ты сегодня идешь в кино?",
                Model = "command-a-03-2025",
            };

            var result = await action.DetectLocale(input);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.Text));
        }

        [TestMethod]
        public async Task CalculateTextsSimilarity_ShouldReturnScore()
        {
            var action = new ChatActions(InvocationContext, FileManager);

            var input = new CalculateTextsSimilarityRequest
            {
                FirstText = "Blackbird connects your apps to automate localization workflows.",
                SecondText = "Our platform integrates tools to streamline localization processes.",
                Model = "embed-english-v3.0"
            };

            var result = await action.CalculateTextsSimilarity(input);

            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented));
            Assert.IsNotNull(result);
        }
       
        [TestMethod]
        public async Task RerankTexts_ShouldReturnOrderedTexts()
        {
            var action = new ChatActions(InvocationContext, FileManager);

            var input = new RerankTextsRequest
            {
                Query = "automating localization workflows",
                Texts = new[]
                {
                "Blackbird integrates your apps to automate localization workflows.",
                "A tasty recipe for apple pie with cinnamon.",
                "Tools that connect systems and reduce manual steps in translation projects."
            },
                TopN = 3,
                MinimumRelevanceScore = 0.2f,
                Model = "rerank-multilingual-v3.0"
            };

            var result = await action.RerankTexts(input);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task RerankTextsProvidedInFile_ShouldReturnOrderedTexts()
        {
            var action = new ChatActions(InvocationContext, FileManager);

            var input = new RerankTextsProvidedInFileRequest
            {
                Query = "automating localization workflows",
                TxtFileWithTexts = new Blackbird.Applications.Sdk.Common.Files.FileReference { Name=""},
                TopN = 3,
                MinimumRelevanceScore = 0.2f,
                Model = "rerank-multilingual-v3.0"
            };

            var result = await action.RerankTextsProvidedInFile(input);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Translate_ShouldReturnOrderedTexts()
        {
            var action = new TranslationActions(InvocationContext, FileManager);

            var input = new CohereTranslateFileRequest
            {
                File= new Blackbird.Applications.Sdk.Common.Files.FileReference { Name = "contentful.html.xliff" },
                TargetLanguage = "es",
            };

            var result = await action.Translate(input);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Translat_ShouldReturnOrderedTexts()
        {
            var action = new TranslationActions(InvocationContext, FileManager);

            var input = new TranslateTextRequest
            {
                Text = "Hello, how are you?",
                TargetLanguage = "es",
            };

            var result = await action.TranslateText(input);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

            Assert.IsNotNull(result);
        }
    }
}
