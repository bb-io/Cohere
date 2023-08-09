using Apps.Cohere.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Cohere.Models.Requests;

public class ClassifyTextWithFileExamplesRequest
{
    [Display("Text to classify")]
    public string Text { get; set; }
    
    [Display("Csv file with examples")]
    public byte[] CsvFileWithExamples { get; set; }
    
    public string Filename { get; set; }
    
    [DataSource(typeof(EmbedModelDataSourceHandler))]
    public string? Model { get; set; }
}