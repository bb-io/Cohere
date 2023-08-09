using Apps.Cohere.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Cohere.Models.Requests;

public class SummarizeTextRequest
{
    [Display("Text to summarize")]
    public string Text { get; set; }
    
    [Display("Length")]
    [DataSource(typeof(SummaryLengthDataSourceHandler))]
    public string? Length { get; set; }
    
    [Display("Format")]
    [DataSource(typeof(SummaryFormatDataSourceHandler))]
    public string? Format { get; set; } 
    
    [DataSource(typeof(SummarizeModelDataSourceHandler))]
    public string? Model { get; set; }
    
    [Display("Extractiveness")]
    [DataSource(typeof(SummaryExtractivenessDataSourceHandler))]
    public string? Extractiveness { get; set; }
    
    [DataSource(typeof(TemperatureDataSourceHandler))]
    public float? Temperature { get; set; }
    
    [Display("Additional command")]
    public string? AdditionalCommand { get; set; }
}