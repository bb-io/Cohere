using Apps.Cohere.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Cohere.Models.Requests;

public class SummariseTextAnalysesRequest
{
    [Display("Text analyses")]
    public IEnumerable<string> TextAnalyses { get; set; }
    
    [DataSource(typeof(GenerateTextModelDataSourceHandler))]
    public string? Model { get; set; }
}