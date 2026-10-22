using Apps.Cohere.Ai;
using Apps.Cohere.DataSourceHandlers;
using Apps.Cohere.Models.Responses;
using Blackbird.Ai.Batching.Models;
using Blackbird.Ai.Content.Models;
using Blackbird.Ai.Files;
using Blackbird.Ai.Files.Models;
using Blackbird.Ai.Llm.Abstract;
using Blackbird.Ai.Llm.Models;
using Blackbird.Ai.Prompts.Models;
using Blackbird.Ai.Text;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;

namespace Apps.Cohere.Actions;

public abstract class BaseActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
    : Invocable(invocationContext)
{
    protected IFileManagementClient FileManagementClient { get; } = fileManagementClient;

    protected virtual ILlmClient CreateLlmClient(string model, CohereLlmSettings settings) =>
        new CohereLlmClient(Client, model, settings);

    protected async Task<string> ResolveModelAsync(string? model, string? fallback = null) =>
        !string.IsNullOrWhiteSpace(model) ? model
        : fallback ?? await Client.ResolveDefaultModelAsync(CohereModelEndpoints.Chat);

    protected ContentStreamProcessor CreateContentProcessor(ILlmClient client, string model, int? bucketSize = null) =>
        new(client, CreateEngineOptions(model, bucketSize));

    protected TextProcessor CreateTextProcessor(ILlmClient client, string model) =>
        new(client, CreateEngineOptions(model));

    protected async Task<Stream?> DownloadOptionalAsync(FileReference? file) =>
        file is null ? null : await FileManagementClient.DownloadAsync(file);

    protected async Task<FileReference> UploadAsync(ContentStream file)
    {
        await using var content = file.Content;
        return await FileManagementClient.UploadAsync(content, file.MediaType, file.FileName);
    }

    protected static UsageResponse ToUsage(LlmUsage usage) => new()
    {
        PromptTokens = usage.InputTokens,
        CompletionTokens = usage.OutputTokens,
        TotalTokens = usage.TotalTokens
    };

    protected static string UserPrompt(ContentResult content) =>
        string.Join(Environment.NewLine, content.Traces.Select(trace => trace.UserPrompt));

    protected static string? Instructions(string? targetAudience, string? additionalInstructions, string? tone = null)
    {
        var instructions = new[]
            {
                string.IsNullOrWhiteSpace(targetAudience) ? null : $"The target audience is {targetAudience.Trim()}.",
                string.IsNullOrWhiteSpace(tone) ? null : $"Use this tone: {tone.Trim()}.",
                additionalInstructions
            }
            .Where(part => !string.IsNullOrWhiteSpace(part))
            .ToList();

        return instructions.Count > 0 ? string.Join(Environment.NewLine, instructions) : null;
    }

    protected static void ThrowIfUnresolved(ContentResult content)
    {
        if (content.UnresolvedSegments > 0)
        {
            throw new PluginApplicationException(string.Join(Environment.NewLine, content.Warnings));
        }
    }

    private EngineOptions CreateEngineOptions(string model, int? bucketSize = null) => new()
    {
        Logger = AiAdapters.Logger(InvocationContext),
        LogTag = "[Cohere]",
        ProvenanceTool = $"Cohere/{model}",
        Bucketing = BucketingOptions.FromInputs(bucketSize, null),
        TraceLevel = PromptTraceLevel.Full
    };
}
