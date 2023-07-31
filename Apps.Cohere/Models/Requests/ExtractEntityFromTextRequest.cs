using Blackbird.Applications.Sdk.Common;

namespace Apps.Cohere.Models.Requests;

public class ExtractEntityFromTextRequest
{
    public string Text { get; set; }
    
    public string Entity { get; set; }
    
    [Display("Maximum number of words")]
    public int? MaximumWordsNumber { get; set; }
    
    public string? Model { get; set; }
    
    [Display("Temperature (from 0.0 to 5.0)")]
    public float? Temperature { get; set; }
}