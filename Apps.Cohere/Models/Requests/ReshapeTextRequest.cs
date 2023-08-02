using Blackbird.Applications.Sdk.Common;

namespace Apps.Cohere.Models.Requests;

public class ReshapeTextRequest
{
    public string Text { get; set; }
    
    [Display("Style, mood and tone")]
    public string ReshapeInstructions { get; set; }
    
    [Display("Maximum number of tokens")]
    public int MaximumTokensNumber { get; set; }
    
    public string? Model { get; set; }
    
    public float? Temperature { get; set; }
}