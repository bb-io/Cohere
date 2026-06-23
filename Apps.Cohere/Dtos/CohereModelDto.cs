using Newtonsoft.Json;

namespace Apps.Cohere.Dtos;

public class CohereModelDto
{
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("is_deprecated")]
    public bool IsDeprecated { get; set; }

    [JsonProperty("endpoints")]
    public IEnumerable<string> Endpoints { get; set; } = Array.Empty<string>();

    [JsonProperty("default_endpoints")]
    public IEnumerable<string> DefaultEndpoints { get; set; } = Array.Empty<string>();
}

public class CohereModelsResponse
{
    [JsonProperty("models")]
    public List<CohereModelDto> Models { get; set; } = new();

    [JsonProperty("next_page_token")]
    public string? NextPageToken { get; set; }
}
