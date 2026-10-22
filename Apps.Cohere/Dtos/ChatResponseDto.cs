using Newtonsoft.Json;

namespace Apps.Cohere.Dtos;

public class ChatResponseDto
{
    [JsonProperty("message")]
    public ChatMessageDto? Message { get; set; }

    [JsonProperty("finish_reason")]
    public string? FinishReason { get; set; }

    [JsonProperty("usage")]
    public ChatUsageDto? Usage { get; set; }
}

public class ChatMessageDto
{
    [JsonProperty("content")]
    public List<ChatContentDto>? Content { get; set; }
}

public class ChatContentDto
{
    [JsonProperty("type")]
    public string? Type { get; set; }

    [JsonProperty("text")]
    public string? Text { get; set; }
}

public class ChatUsageDto
{
    [JsonProperty("billed_units")]
    public TokenCountDto? BilledUnits { get; set; }

    [JsonProperty("tokens")]
    public TokenCountDto? Tokens { get; set; }
}

public class TokenCountDto
{
    [JsonProperty("input_tokens")]
    public double InputTokens { get; set; }

    [JsonProperty("output_tokens")]
    public double OutputTokens { get; set; }
}
