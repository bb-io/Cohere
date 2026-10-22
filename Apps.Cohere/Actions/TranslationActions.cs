using Apps.Cohere.Ai;
using Apps.Cohere.Models.Requests;
using Apps.Cohere.Models.Responses;
using Blackbird.Ai.Files.Models;
using Blackbird.Ai.Summary.Models;
using Blackbird.Ai.Text.Models;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Blueprints;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;

namespace Apps.Cohere.Actions;

[ActionList("Translation")]
public class TranslationActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
    : BaseActions(invocationContext, fileManagementClient)
{
    private const string DefaultTranslationModel = "command-a-translate-08-2025";

    [BlueprintActionDefinition(BlueprintAction.TranslateFile)]
    [Action("Translate", Description = "Translate file content retrieved from a CMS or file storage")]
    public async Task<FileTranslationResponse> Translate([ActionParameter] CohereTranslateFileRequest input)
    {
        var model = await ResolveModelAsync(input.Model, DefaultTranslationModel);
        var client = CreateLlmClient(model, new(MaxTokens: input.MaxTokens));
        var processor = CreateContentProcessor(client, model, input.BucketSize);

        await using var file = await FileManagementClient.DownloadAsync(input.File);
        await using var glossaryFile = await DownloadOptionalAsync(input.Glossary);

        var result = await AiAdapters.Mapped(() => processor.TranslateAsync(new TranslateStreamOptions
        {
            File = file,
            FileName = input.File.Name,
            MediaType = input.File.ContentType,
            SourceLanguage = input.SourceLanguage,
            TargetLanguage = input.TargetLanguage,
            OutputFileHandling = input.OutputFileHandling,
            GlossaryTbx = glossaryFile,
            AdditionalInstructions = Instructions(null, input.Instructions, input.Tone),
            Summary = SummaryOptions.FromToggle(input.GenerateDocumentSummary)
        }));

        var content = result.Content;

        return new FileTranslationResponse
        {
            File = await UploadAsync(result.File),
            TotalSegmentsCount = content.TotalSegments,
            TotalTranslatable = content.ProcessableSegments,
            TargetsUpdatedCount = content.UpdatedSegments,
            ProcessedBatchesCount = content.BucketsProcessed,
            SystemPrompt = content.SystemPrompt,
            Usage = ToUsage(content.Usage)
        };
    }

    [BlueprintActionDefinition(BlueprintAction.TranslateText)]
    [Action("Translate text", Description = "Localize the text provided.")]
    public async Task<TranslateTextResponse> TranslateText([ActionParameter] TranslateTextRequest input)
    {
        var model = await ResolveModelAsync(input.Model, DefaultTranslationModel);
        var client = CreateLlmClient(model, new(MaxTokens: input.MaxTokens));
        var processor = CreateTextProcessor(client, model);

        await using var glossaryFile = await DownloadOptionalAsync(input.Glossary);

        var result = await AiAdapters.Mapped(() => processor.TranslateAsync(new TranslateTextOptions
        {
            Text = input.Text,
            SourceLanguage = input.SourceLanguage,
            TargetLanguage = input.TargetLanguage,
            AdditionalInstructions = input.AdditionalInstructions,
            GlossaryTbx = glossaryFile
        }));

        ThrowIfUnresolved(result.Content);

        return new TranslateTextResponse
        {
            TranslatedText = result.Text,
            SystemPrompt = result.Content.SystemPrompt ?? string.Empty,
            UserPrompt = UserPrompt(result.Content),
            Usage = ToUsage(result.Content.Usage)
        };
    }
}
