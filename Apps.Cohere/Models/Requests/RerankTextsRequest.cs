using Blackbird.Applications.Sdk.Common;

namespace Apps.Cohere.Models.Requests;

public class RerankTextsRequest
{
    public string Query { get; set; }
    
    public IEnumerable<string> Texts { get; set; } 
    
    public string? Model { get; set; }
    
    [Display("Number of most relevant texts to include")]
    public int? TopN { get; set; }
    
    [Display("Minimum relevance score (from 0.0 to 1.0)")]
    public float? MinimumRelevanceScore { get; set; }
}