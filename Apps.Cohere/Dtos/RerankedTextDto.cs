using Apps.Cohere.Converters;
using Newtonsoft.Json;

namespace Apps.Cohere.Dtos;

public class RerankedTextDto
{
    [JsonProperty("document")]
    [JsonConverter(typeof(RerankedTextConverter))]
    public string Text { get; set; }
    
    [JsonProperty("relevance_score")]
    public float RelevanceScore { get; set; }
}