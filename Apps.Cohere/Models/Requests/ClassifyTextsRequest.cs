using Apps.Cohere.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Cohere.Models.Requests;

public class ClassifyTextsRequest
{
    [Display("Text to classify")]
    public string Text { get; set; }
    
    [Display("Example texts")]
    public IEnumerable<string> ExampleTexts { get; set; }
    
    [Display("Example labels")]
    public IEnumerable<string> ExampleLabels { get; set; }
    
    [DataSource(typeof(EmbedModelDataSourceHandler))]
    public string? Model { get; set; }
}