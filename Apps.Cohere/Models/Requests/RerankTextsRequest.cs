using Apps.Cohere.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Cohere.Models.Requests;

public class RerankTextsRequest
{
    public string Query { get; set; }
    
    public IEnumerable<string> Texts { get; set; } 
    
    [DataSource(typeof(RerankModelDataSourceHandler))]
    public string? Model { get; set; }
    
    [Display("Number of most relevant texts to include")]
    public int? TopN { get; set; }
    
    [Display("Minimum relevance score (from 0.0 to 1.0)")]
    [DataSource(typeof(RelevanceScoreDataSourceHandler))]
    public float? MinimumRelevanceScore { get; set; }
}