using Newtonsoft.Json;

namespace Apps.Cohere.Dtos;

public class CohereModelDto
{
    [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("is_deprecated", NullValueHandling = NullValueHandling.Ignore)]
    public bool IsDeprecated { get; set; }

    [JsonProperty("endpoints", NullValueHandling = NullValueHandling.Ignore)]
    public IEnumerable<string> Endpoints { get; set; } = Array.Empty<string>();

    [JsonProperty("default_endpoints", NullValueHandling = NullValueHandling.Ignore)]
    public IEnumerable<string> DefaultEndpoints { get; set; } = Array.Empty<string>();
}

public class CohereModelsResponse
{
    [JsonProperty("models", NullValueHandling = NullValueHandling.Ignore)]
    public List<CohereModelDto> Models { get; set; } = new();

    [JsonProperty("next_page_token", NullValueHandling = NullValueHandling.Ignore)]
    public string? NextPageToken { get; set; }
}
