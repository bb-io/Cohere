using Blackbird.Applications.Sdk.Common;

namespace Apps.Cohere.Models.Requests;

public class SummariseTextAnalysesRequest
{
    [Display("Text analyses")]
    public IEnumerable<string> TextAnalyses { get; set; }
    
    public string? Model { get; set; }
}