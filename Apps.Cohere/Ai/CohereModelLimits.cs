using Blackbird.Ai.Llm.Models;

namespace Apps.Cohere.Ai;

public static class CohereModelLimits
{
    private static readonly (int ContextTokens, int MaxOutputTokens) Unknown = (8_192, 4_096);

    private static readonly (string Prefix, int ContextTokens, int MaxOutputTokens)[] Models =
    [
        ("command-a-plus", 436_000, 64_000),
        ("command-a-reasoning", 288_768, 32_768),
        ("command-a-translate", 8_992, 8_000),
        ("command-a-vision", 128_000, 8_192),
        ("command-a", 288_000, 8_192),
        ("command-r", 128_000, 4_096),
        ("c4ai-aya-vision", 16_384, 4_096),
        ("c4ai-aya-expanse", 128_000, 4_096),
        ("north-small-translate", 32_768, 16_384),
        ("north-mini-code", 436_000, 64_000),
        ("tiny-aya", 8_192, 8_192)
    ];

    public static LlmCapabilities Capabilities(string model, int? requestedMaxTokens)
    {
        var limits = Models
            .Where(entry => model.StartsWith(entry.Prefix, StringComparison.OrdinalIgnoreCase))
            .Select(entry => (entry.ContextTokens, entry.MaxOutputTokens))
            .DefaultIfEmpty(Unknown)
            .First();

        var maxOutputTokens = Math.Min(limits.MaxOutputTokens, limits.ContextTokens / 2);
        if (requestedMaxTokens is > 0)
            maxOutputTokens = Math.Min(requestedMaxTokens.Value, maxOutputTokens);

        return new LlmCapabilities(
            SupportsStructuredOutput: false,
            ContextTokens: limits.ContextTokens - maxOutputTokens,
            MaxOutputTokens: maxOutputTokens);
    }
}
