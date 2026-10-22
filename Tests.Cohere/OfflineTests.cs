using Apps.Cohere.Actions;
using Apps.Cohere.Ai;
using Apps.Cohere.Models.Requests;
using Blackbird.Ai.Llm.Abstract;
using Blackbird.Ai.Llm.Models;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Tests.Cohere.Base;

namespace Tests.Cohere;

[TestClass]
public class OfflineTests
{
    private const string Model = "command-a-03-2025";

    private readonly InMemoryFileManager _files = new();
    private readonly List<string> _loggedWarnings = [];

    [TestMethod]
    public async Task Translate_UnusableAnswers_LogsWarningsAndLeavesSegmentsUntouched()
    {
        var file = _files.Add("broken.xlf", InMemoryFileManager.Xliff("en", "de", ("One", null), ("Two", null)));

        var response = await new UnusableAnswerTranslationActions(Context(), _files).Translate(
            new CohereTranslateFileRequest { File = file, Model = Model, TargetLanguage = "de" });

        _loggedWarnings.ForEach(Console.WriteLine);
        Assert.AreEqual(0, response.TargetsUpdatedCount);
        Assert.IsTrue(_loggedWarnings.Count > 0 && _loggedWarnings.All(warning => warning.Contains("[Cohere]")));
        Assert.IsTrue(_files.Segments(response.File).All(segment => string.IsNullOrEmpty(segment.GetTarget())));
    }

    [TestMethod]
    public async Task TranslateText_UnusableAnswers_Throws()
    {
        var exception = await Assert.ThrowsExceptionAsync<PluginApplicationException>(() =>
            new UnusableAnswerTranslationActions(Context(), _files).TranslateText(
                new TranslateTextRequest { Model = Model, Text = "Hello", SourceLanguage = "en", TargetLanguage = "de" }));

        Console.WriteLine(exception.Message);
        Assert.IsTrue(_loggedWarnings.Count > 0);
    }

    [TestMethod]
    public async Task EditText_UnusableAnswers_Throws()
    {
        await Assert.ThrowsExceptionAsync<PluginApplicationException>(() =>
            new UnusableAnswerEditActions(Context(), _files).EditText(
                new EditTranslatedTextRequest { Model = Model, SourceText = "Hello", TargetText = "Hallo", SourceLanguage = "en", TargetLanguage = "de" }));

        Assert.IsTrue(_loggedWarnings.Count > 0);
    }

    [TestMethod]
    public async Task ReviewText_UnusableAnswers_Throws()
    {
        await Assert.ThrowsExceptionAsync<PluginApplicationException>(() =>
            new UnusableAnswerReviewActions(Context(), _files).ReviewText(
                new ReviewTextRequest { Model = Model, SourceText = "Hello", TargetText = "Hallo", SourceLanguage = "en", TargetLanguage = "de" }));

        Assert.IsTrue(_loggedWarnings.Count > 0);
    }

    [TestMethod]
    public async Task SummarizeText_EmptyAnswer_Throws()
    {
        await Assert.ThrowsExceptionAsync<PluginApplicationException>(() =>
            new FixedAnswerSummarizeActions(Context(), _files, string.Empty).SummarizeText(
                new SummarizeTextRequest { Model = Model, Text = "A text worth summarizing." }));
    }

    [TestMethod]
    public async Task ShortenText_NeverFits_Throws()
    {
        var exception = await Assert.ThrowsExceptionAsync<PluginApplicationException>(() =>
            new FixedAnswerShortenActions(Context(), _files, """{"translations":[{"translation_id":"1","translated_text":"Still a sentence that is far too long."}]}""").ShortenText(
                new ShortenTextRequest { Model = Model, Text = "A sentence that is definitely far too long.", MaximumLength = 10, Retries = 1 }));

        Console.WriteLine(exception.Message);
        Assert.IsTrue(_loggedWarnings.Count > 0);
    }

    [TestMethod]
    public async Task ShortenText_NonPositiveMaximumLength_Throws()
    {
        await Assert.ThrowsExceptionAsync<PluginMisconfigurationException>(() =>
            new FixedAnswerShortenActions(Context(), _files, "unused").ShortenText(
                new ShortenTextRequest { Model = Model, Text = "Text", MaximumLength = 0 }));
    }

    [TestMethod]
    public async Task ShortenContent_InvalidInputs_Throw()
    {
        var action = new FixedAnswerShortenActions(Context(), _files, "unused");

        await Assert.ThrowsExceptionAsync<PluginMisconfigurationException>(() =>
            action.ShortenContent(new ShortenContentRequest { Model = Model, MaximumLength = -1 }));
        await Assert.ThrowsExceptionAsync<PluginMisconfigurationException>(() =>
            action.ShortenContent(new ShortenContentRequest { Model = Model, Retries = -1 }));
    }

    [TestMethod]
    public async Task ReviewFile_ThresholdOutOfRange_Throws()
    {
        await Assert.ThrowsExceptionAsync<PluginMisconfigurationException>(() =>
            new UnusableAnswerReviewActions(Context(), _files).ReviewFile(new ReviewFileRequest { Model = Model, Threshold = 1.5 }));
    }

    [TestMethod]
    public async Task ReviewText_EmptyTarget_Throws()
    {
        await Assert.ThrowsExceptionAsync<PluginMisconfigurationException>(() =>
            new UnusableAnswerReviewActions(Context(), _files).ReviewText(
                new ReviewTextRequest { Model = Model, SourceText = "Hello", TargetText = " ", SourceLanguage = "en", TargetLanguage = "de" }));
    }

    [TestMethod]
    public async Task Translate_NoTargetLanguage_Throws()
    {
        var file = _files.Add("no-target.xlf", InMemoryFileManager.Xliff("en", "de", ("One", null)).Replace(" trgLang=\"de\"", string.Empty));

        await Assert.ThrowsExceptionAsync<PluginMisconfigurationException>(() =>
            new UnusableAnswerTranslationActions(Context(), _files).Translate(
                new CohereTranslateFileRequest { File = file, Model = Model }));
    }

    private InvocationContext Context() => new()
    {
        AuthenticationCredentialsProviders = [new AuthenticationCredentialsProvider("Authorization", "Bearer unused")],
        Logger = new Logger("Debug") { LogWarning = (message, _) => _loggedWarnings.Add(message ?? string.Empty) }
    };

    private sealed class UnusableAnswerTranslationActions(InvocationContext context, IFileManagementClient files)
        : TranslationActions(context, files)
    {
        protected override ILlmClient CreateLlmClient(string model, CohereLlmSettings settings) => new FixedAnswerClient("not json");
    }

    private sealed class UnusableAnswerEditActions(InvocationContext context, IFileManagementClient files)
        : EditActions(context, files)
    {
        protected override ILlmClient CreateLlmClient(string model, CohereLlmSettings settings) => new FixedAnswerClient("not json");
    }

    private sealed class UnusableAnswerReviewActions(InvocationContext context, IFileManagementClient files)
        : ReviewActions(context, files)
    {
        protected override ILlmClient CreateLlmClient(string model, CohereLlmSettings settings) => new FixedAnswerClient("not json");
    }

    private sealed class FixedAnswerShortenActions(InvocationContext context, IFileManagementClient files, string answer)
        : ShortenActions(context, files)
    {
        protected override ILlmClient CreateLlmClient(string model, CohereLlmSettings settings) => new FixedAnswerClient(answer);
    }

    private sealed class FixedAnswerSummarizeActions(InvocationContext context, IFileManagementClient files, string answer)
        : SummarizeActions(context, files)
    {
        protected override ILlmClient CreateLlmClient(string model, CohereLlmSettings settings) => new FixedAnswerClient(answer);
    }

    private sealed class FixedAnswerClient(string answer) : ILlmClient
    {
        public LlmCapabilities Capabilities { get; } = new(ContextTokens: 128_000, MaxOutputTokens: 8_192);

        public Task<LlmResponse> CompleteAsync(LlmRequest request, CancellationToken cancellationToken) =>
            Task.FromResult(LlmResponse.Completed(answer, new LlmUsage(10, 2)));
    }
}
