using Apps.Cohere;
using Apps.Cohere.Models.Requests;
using Newtonsoft.Json;
using Tests.Cohere.Base;

namespace Tests.Cohere
{
    [TestClass]
    public class ActionTest : TestBase
    {
        [TestMethod]
        public async Task GenerateText_IssSuccess()
        {
            var action = new Actions(InvocationContext,FileManager);

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
            var action = new Actions(InvocationContext, FileManager);

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
            var action = new Actions(InvocationContext, FileManager);

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
            var action = new Actions(InvocationContext, FileManager);

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
            var action = new Actions(InvocationContext, FileManager);

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
            var action = new Actions(InvocationContext, FileManager);

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
    }
}
