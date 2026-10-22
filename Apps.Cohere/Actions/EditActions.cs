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

[ActionList("Editing")]
public class EditActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
    : BaseActions(invocationContext, fileManagementClient)
{
    [BlueprintActionDefinition(BlueprintAction.EditFile)]
    [Action("Edit", Description = "Edit a translation. Assumes you have previously translated content in Blackbird through any translation action.")]
    public async Task<EditFileResponse> EditFile([ActionParameter] EditFileRequest input)
    {
        var model = await ResolveModelAsync(input.Model);
        var client = CreateLlmClient(model, new(input.Temperature, input.TopP, input.MaxTokens));
        var processor = CreateContentProcessor(client, model, input.BucketSize);

        await using var file = await FileManagementClient.DownloadAsync(input.File);
        await using var glossaryFile = await DownloadOptionalAsync(input.Glossary);

        var result = await AiAdapters.Mapped(() => processor.EditAsync(new EditStreamOptions
        {
            File = file,
            FileName = input.File.Name,
            MediaType = input.File.ContentType,
            SourceLanguage = input.SourceLanguage,
            TargetLanguage = input.TargetLanguage,
            OutputFileHandling = input.OutputFileHandling,
            GlossaryTbx = glossaryFile,
            AdditionalInstructions = input.AdditionalInstructions,
            Summary = SummaryOptions.FromToggle(input.GenerateDocumentSummary)
        }));

        return new EditFileResponse
        {
            File = await UploadAsync(result.File),
            Usage = ToUsage(result.Content.Usage),
            TotalSegmentsReviewed = result.Content.ProcessableSegments,
            TotalSegmentsUpdated = result.Content.UpdatedSegments
        };
    }

    [BlueprintActionDefinition(BlueprintAction.EditText)]
    [Action("Edit text", Description = "Review translated text and generate an edited version")]
    public async Task<EditTranslatedTextResponse> EditText([ActionParameter] EditTranslatedTextRequest input)
    {
        var model = await ResolveModelAsync(input.Model);
        var client = CreateLlmClient(model, new(input.Temperature, input.TopP, input.MaxTokens));
        var processor = CreateTextProcessor(client, model);

        await using var glossaryFile = await DownloadOptionalAsync(input.Glossary);

        var result = await AiAdapters.Mapped(() => processor.EditAsync(new EditTextOptions
        {
            SourceText = input.SourceText,
            TargetText = input.TargetText,
            SourceLanguage = input.SourceLanguage,
            TargetLanguage = input.TargetLanguage,
            AdditionalInstructions = Instructions(input.TargetAudience, input.AdditionalInstructions),
            GlossaryTbx = glossaryFile
        }));

        ThrowIfUnresolved(result.Content);

        return new EditTranslatedTextResponse
        {
            EditedText = result.Text,
            SystemPrompt = result.Content.SystemPrompt ?? string.Empty,
            UserPrompt = UserPrompt(result.Content),
            Usage = ToUsage(result.Content.Usage)
        };
    }
}
