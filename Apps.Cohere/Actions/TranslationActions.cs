using Apps.Cohere.Models.Requests;
using Apps.Cohere.Models.Responses;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Filters.Constants;
using Blackbird.Filters.Enums;
using Blackbird.Filters.Transformations;
using RestSharp;
using Blackbird.Filters.Extensions;
using Blackbird.Applications.SDK.Blueprints;

namespace Apps.Cohere.Actions;

[ActionList("Translation")]
public class TranslationActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
    : Invocable(invocationContext)
{
    [BlueprintActionDefinition(BlueprintAction.TranslateFile)]
    [Action("Translate", Description = "Translate a file ")]
    public async Task<FileTranslationResponse> Translate([ActionParameter] CohereTranslateFileRequest input)
    {
        await using var stream = await fileManagementClient.DownloadAsync(input.File);
        var content = await Transformation.Parse(stream, input.File.Name);
        return await HandleCohereInteroperableTransformation(content, input);
    }

    private async Task<FileTranslationResponse> HandleCohereInteroperableTransformation(
        Transformation content, CohereTranslateFileRequest input)
    {
        content.SourceLanguage ??= input.SourceLanguage;
        content.TargetLanguage ??= input.TargetLanguage;

        if (string.IsNullOrWhiteSpace(content.TargetLanguage))
            throw new PluginMisconfigurationException("Target language is required.");

        async Task<IEnumerable<string>> BatchTranslate(IEnumerable<Segment> batch)
        {
            var list = new List<string>();

            foreach (var seg in batch)
            {
                var srcText = seg.GetSource();
                if (string.IsNullOrWhiteSpace(srcText))
                {
                    list.Add(srcText ?? string.Empty);
                    continue;
                }

                var preserve = input.PreserveFormatting == true
                    ? "Preserve original formatting and line breaks."
                    : "You may normalize spacing if needed.";

                var srcLangLine = string.IsNullOrWhiteSpace(content.SourceLanguage)
                    ? "" : $"Source language: {content.SourceLanguage}\n";

                var toneLine = string.IsNullOrWhiteSpace(input.Tone)
                    ? "" : $"Tone: {input.Tone}\n";

                var instLine = string.IsNullOrWhiteSpace(input.Instructions)
                    ? "" : $"Additional instructions: {input.Instructions}\n";

                var prompt =
                    $"Translate the following text into {content.TargetLanguage}.\n" +
                    $"{preserve}\n" +
                    $"{srcLangLine}" +
                    $"{toneLine}" +
                    $"{instLine}" +
                    "Return only the translation, with no extra words or labels.\n\n" +
                    $"Text:\n{srcText}";

                var model = input.Model ?? "command-a-translate-08-2025";

                var request = new CohereRequest("/chat", Method.Post, Creds);
                request.AddJsonBody(new
                {
                    message = prompt,
                    model = model,
                    max_tokens = input.MaxTokens.GetValueOrDefault(1024),
                    temperature = 0.0f
                });

                var resp = await Client.ExecuteWithErrorHandling<TranslateTextResponse>(request);
                list.Add(resp.TranslatedText?.Trim() ?? string.Empty);
            }

            return list;
        }

        var segments = content.GetSegments()
            .Where(s => !s.IsIgnorbale && s.IsInitial)
            .ToList();

        var segmentTranslations = await segments.Batch(100).Process(BatchTranslate);

        foreach (var (segment, translated) in segmentTranslations)
        {
            if (!string.IsNullOrEmpty(translated))
            {
                segment.SetTarget(translated);
                segment.State = SegmentState.Translated;
            }
        }

        var outputMode = (input.OutputFileHandling ?? "").ToLowerInvariant();

        if (outputMode == "original")
        {
            var targetContent = content.Target();
            var outFile = await fileManagementClient.UploadAsync(
                targetContent.Serialize().ToStream(),
                targetContent.OriginalMediaType ?? "application/octet-stream",
                targetContent.OriginalName ?? input.File.Name);

            return new FileTranslationResponse { File = outFile };
        }

        var xliff = content.Serialize();
        await using var memoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(xliff));
        var xliffRef = await fileManagementClient.UploadAsync(memoryStream, MediaTypes.Xliff, content.XliffFileName);
        return new FileTranslationResponse { File = xliffRef };
    }

    [BlueprintActionDefinition(BlueprintAction.TranslateText)]
    [Action("Translate text", Description = "Translate input text with Command A Translate.")]
    public async Task<TranslateTextResponse> TranslateText([ActionParameter] TranslateTextRequest input)
    {
        var model = input.Model ?? "command-a-translate-08-2025";

        var format = input.PreserveFormatting == true
            ? "Preserve original formatting, line breaks and inline punctuation."
            : "You may normalize spacing if needed.";

        var source = string.IsNullOrWhiteSpace(input.SourceLanguage)
            ? ""
            : $"Source language: {input.SourceLanguage}\n";

        var prompt =
            $"Translate the following text into {input.TargetLanguage}. {format}\n" +
            $"{source}\n" +
            "Return only the translation, with no additional words or labels.\n\n" +
            $"Text:\n{input.Text}";

        var request = new CohereRequest("/chat", Method.Post, Creds);
        request.AddJsonBody(new
        {
            message = prompt,
            model = model,
            max_tokens = input.MaxTokens.GetValueOrDefault(1024),
            temperature = 0.0f
        });

        var resp = await Client.ExecuteWithErrorHandling<TranslateTextResponse>(request);
        return new TranslateTextResponse { TranslatedText = resp.TranslatedText?.Trim() ?? string.Empty };
    }
}

