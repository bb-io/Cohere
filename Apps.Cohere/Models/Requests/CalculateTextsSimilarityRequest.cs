using Blackbird.Applications.Sdk.Common;

namespace Apps.Cohere.Models.Requests;

public class CalculateTextsSimilarityRequest
{
    [Display("First text")]
    public string FirstText { get; set; }
    
    [Display("Second text")]
    public string SecondText { get; set; }
    
    public string? Model { get; set; }
}