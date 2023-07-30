using Blackbird.Applications.Sdk.Common;

namespace Apps.Cohere.Models.Requests;

public class SummarizeTextRequest
{
    [Display("Text to summarize")]
    public string Text { get; set; }
    
    [Display("Length: short/medium/long/auto")]
    public string? Length { get; set; }
    
    [Display("Format: paragraph/bullets/auto")]
    public string? Format { get; set; } 
    
    public string? Model { get; set; }
    
    [Display("Extractiveness: low/medium/high/auto")]
    public string? Extractiveness { get; set; }
    
    public float? Temperature { get; set; }
    
    [Display("Additional command")]
    public string? AdditionalCommand { get; set; }
}