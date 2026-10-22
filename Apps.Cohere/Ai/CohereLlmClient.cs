using Apps.Cohere.Dtos;
using Blackbird.Ai.Llm.Abstract;
using Blackbird.Ai.Llm.Models;
using Newtonsoft.Json;

namespace Apps.Cohere.Ai;

public sealed class CohereLlmClient(CohereClient client, string model, CohereLlmSettings settings) : ILlmClient
{
    private static readonly JsonSerializerSettings BodySettings = new() { NullValueHandling = NullValueHandling.Ignore };

    public LlmCapabilities Capabilities { get; } = CohereModelLimits.Capabilities(model, settings.MaxTokens);

    public async Task<LlmResponse> CompleteAsync(LlmRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var messages = new[]
            {
                new { role = "system", content = request.SystemPrompt },
                new { role = "user", content = request.UserPrompt }
            }
            .Where(message => !string.IsNullOrEmpty(message.content));

        var body = JsonConvert.SerializeObject(new
        {
            model,
            messages,
            max_tokens = Capabilities.MaxOutputTokens,
            temperature = settings.Temperature,
            p = settings.TopP
        }, BodySettings);

        var response = await client.ChatAsync(body);

        var text = string.Concat((response.Message?.Content ?? [])
            .Where(content => content.Type == "text")
            .Select(content => content.Text));

        return new LlmResponse(
            text,
            ToUsage(response.Usage),
            response.FinishReason == "MAX_TOKENS" ? FinishReason.Truncated : FinishReason.Stop);
    }

    private static LlmUsage ToUsage(ChatUsageDto? usage) => (usage?.BilledUnits ?? usage?.Tokens) is { } tokens
        ? new LlmUsage((int)tokens.InputTokens, (int)tokens.OutputTokens)
        : new LlmUsage(0, 0);
}
