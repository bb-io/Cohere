using Apps.Cohere.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Cohere.Models.Requests;

public class ClassifyTextWithFileExamplesRequest
{
    [Display("Text to classify")]
    public string Text { get; set; }
    
    [Display("Csv file with examples")]
    public FileReference CsvFileWithExamples { get; set; }
    
    [DataSource(typeof(EmbedModelDataSourceHandler))]
    public string? Model { get; set; }
}