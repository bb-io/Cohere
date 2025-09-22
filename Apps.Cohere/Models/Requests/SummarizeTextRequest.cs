using Apps.Cohere.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Cohere.Models.Requests;

public class SummarizeTextRequest
{
    [Display("Text to summarize")]
    public string Text { get; set; }
    
    [Display("Length")]
    [StaticDataSource(typeof(SummaryLengthDataSourceHandler))]
    public string? Length { get; set; }
    
    [Display("Format")]
    [StaticDataSource(typeof(SummaryFormatDataSourceHandler))]
    public string? Format { get; set; } 
    
    [StaticDataSource(typeof(SummarizeModelDataSourceHandler))]
    public string? Model { get; set; }
    
    [Display("Extractiveness")]
    [StaticDataSource(typeof(SummaryExtractivenessDataSourceHandler))]
    public string? Extractiveness { get; set; }
    
    [DataSource(typeof(TemperatureDataSourceHandler))]
    public float? Temperature { get; set; }
    
    [Display("Additional command")]
    public string? AdditionalCommand { get; set; }
}