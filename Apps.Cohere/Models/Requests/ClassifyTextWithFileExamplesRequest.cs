using Apps.Cohere.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.Cohere.Models.Requests;

public class ClassifyTextWithFileExamplesRequest
{
    [Display("Text to classify")]
    public string Text { get; set; }
    
    [Display("Csv file with examples")]
    public File CsvFileWithExamples { get; set; }
    
    [DataSource(typeof(EmbedModelDataSourceHandler))]
    public string? Model { get; set; }
}