namespace Apps.Cohere.Ai;

public sealed record CohereLlmSettings(double? Temperature = null, double? TopP = null, int? MaxTokens = null);
