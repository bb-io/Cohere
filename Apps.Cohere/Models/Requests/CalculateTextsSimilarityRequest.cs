using Apps.Cohere.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Cohere.Models.Requests;

public class CalculateTextsSimilarityRequest
{
    [Display("First text")]
    public string FirstText { get; set; }
    
    [Display("Second text")]
    public string SecondText { get; set; }
    
    [DataSource(typeof(EmbedModelDataSourceHandler))]
    public string? Model { get; set; }
}