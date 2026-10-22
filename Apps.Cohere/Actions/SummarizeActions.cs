using Apps.Cohere.Ai;
using Apps.Cohere.Models.Requests;
using Apps.Cohere.Models.Responses;
using Blackbird.Ai.Summarizing.Models;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;

namespace Apps.Cohere.Actions;

[ActionList("Summarizing")]
public class SummarizeActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
    : BaseActions(invocationContext, fileManagementClient)
{
    [Action("Summarize", Description = "Summarize file content for a target audience and language. Translated content is summarized from its translation.")]
    public async Task<SummarizeResponse> Summarize([ActionParameter] SummarizeContentRequest input)
    {
        var model = await ResolveModelAsync(input.Model);
        var client = CreateLlmClient(model, new());
        var processor = CreateContentProcessor(client, model);

        await using var file = await FileManagementClient.DownloadAsync(input.File);
        await using var glossaryFile = await DownloadOptionalAsync(input.Glossary);

        var result = await AiAdapters.Mapped(() => processor.SummarizeAsync(new SummarizeStreamOptions
        {
            File = file,
            FileName = input.File.Name,
            MediaType = input.File.ContentType,
            Language = input.Language,
            MaxWords = input.MaximumWords,
            AdditionalInstructions = Instructions(input.TargetAudience, input.AdditionalInstructions),
            GlossaryTbx = glossaryFile
        }));

        return ToResponse(result);
    }

    [Action("Summarize text", Description = "Summarize text for a target audience and language.")]
    public async Task<SummarizeResponse> SummarizeText([ActionParameter] SummarizeTextRequest input)
    {
        var model = await ResolveModelAsync(input.Model);
        var client = CreateLlmClient(model, new());
        var processor = CreateTextProcessor(client, model);

        await using var glossaryFile = await DownloadOptionalAsync(input.Glossary);

        var result = await AiAdapters.Mapped(() => processor.SummarizeAsync(new SummarizeTextOptions
        {
            Text = input.Text,
            Language = input.Language,
            MaxWords = input.MaximumWords,
            AdditionalInstructions = Instructions(input.TargetAudience, input.AdditionalInstructions),
            GlossaryTbx = glossaryFile
        }));

        if (string.IsNullOrWhiteSpace(result.Summary) && !string.IsNullOrWhiteSpace(input.Text))
            throw new PluginApplicationException(string.Join(Environment.NewLine, result.Warnings));

        return ToResponse(result);
    }

    private static SummarizeResponse ToResponse(SummarizeResult result) => new()
    {
        Summary = result.Summary,
        SystemPrompt = result.SystemPrompt ?? string.Empty,
        Usage = ToUsage(result.Usage)
    };
}
