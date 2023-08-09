using Apps.Cohere.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Cohere.Models.Requests;

public class ExtractEntityFromTextRequest
{
    public string Text { get; set; }
    
    public string Entity { get; set; }
    
    [Display("Maximum number of tokens")]
    public int? MaximumTokensNumber { get; set; }
    
    [DataSource(typeof(GenerateTextModelDataSourceHandler))]
    public string? Model { get; set; }
    
    [Display("Temperature (from 0.0 to 5.0)")]
    [DataSource(typeof(TemperatureDataSourceHandler))]
    public float? Temperature { get; set; }
}