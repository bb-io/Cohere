using Blackbird.Applications.Sdk.Common;

namespace Apps.Cohere.Models.Requests;

public class ClassifyTextsRequest
{
    [Display("Text to classify")]
    public string Text { get; set; }
    
    [Display("Example texts")]
    public IEnumerable<string> ExampleTexts { get; set; }
    
    [Display("Example labels")]
    public IEnumerable<string> ExampleLabels { get; set; }
    
    public string? Model { get; set; }
}