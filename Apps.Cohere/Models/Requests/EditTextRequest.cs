using Apps.Cohere.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Cohere.Models.Requests;

public class EditTextRequest
{
    public string Text { get; set; }
    
    public string Instruction { get; set; }
    
    [Display("Maximum number of tokens")]
    public int MaximumTokensNumber { get; set; }
    
    [DataSource(typeof(GenerateTextModelDataSourceHandler))]
    public string? Model { get; set; }
    
    [Display("Temperature (from 0.0 to 5.0)")]
    [DataSource(typeof(TemperatureDataSourceHandler))]
    public float? Temperature { get; set; }
    
    [Display("Top-k (from 0 to 500)")]
    public int? TopK { get; set; }
    
    [Display("Top-p (from 0.0 to 1.0)")]
    [DataSource(typeof(TopPDataSourceHandler))]
    public float? TopP { get; set; }
    
    [Display("Frequency penalty (from 0.0 to 1.0)")]
    [DataSource(typeof(PenaltyDataSourceHandler))]
    public float? FrequencyPenalty { get; set; }
    
    [Display("Presence penalty (from 0.0 to 1.0)")]
    [DataSource(typeof(PenaltyDataSourceHandler))]
    public float? PresencePenalty { get; set; }
    
    [Display("Stop sequences")]
    public IEnumerable<string>? StopSequences { get; set; }
}