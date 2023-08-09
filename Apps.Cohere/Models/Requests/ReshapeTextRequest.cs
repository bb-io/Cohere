using Apps.Cohere.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Cohere.Models.Requests;

public class ReshapeTextRequest
{
    public string Text { get; set; }
    
    [Display("Style, mood and tone")]
    public string ReshapeInstructions { get; set; }
    
    [Display("Maximum number of tokens")]
    public int MaximumTokensNumber { get; set; }
    
    [Display("Additional instruction")]
    public string? AdditionalInstruction { get; set; }
    
    [DataSource(typeof(GenerateTextModelDataSourceHandler))]
    public string? Model { get; set; }
    
    [DataSource(typeof(TemperatureDataSourceHandler))]
    public float? Temperature { get; set; }
}