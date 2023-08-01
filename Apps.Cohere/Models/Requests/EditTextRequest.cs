using Blackbird.Applications.Sdk.Common;

namespace Apps.Cohere.Models.Requests;

public class EditTextRequest
{
    public string Text { get; set; }
    
    public string Instruction { get; set; }
    
    [Display("Maximum number of tokens")]
    public int MaximumTokensNumber { get; set; }
    
    public string? Model { get; set; }
    
    [Display("Temperature (from 0.0 to 5.0)")]
    public float? Temperature { get; set; }
    
    [Display("Top-k (from 0 to 500)")]
    public int? TopK { get; set; }
    
    [Display("Top-p (from 0.0 to 1.0)")]
    public float? TopP { get; set; }
    
    [Display("Frequency penalty (from 0.0 to 1.0)")]
    public float? FrequencyPenalty { get; set; }
    
    [Display("Presence penalty (from 0.0 to 1.0)")]
    public float? PresencePenalty { get; set; }
    
    [Display("Stop sequences")]
    public IEnumerable<string>? StopSequences { get; set; }
}