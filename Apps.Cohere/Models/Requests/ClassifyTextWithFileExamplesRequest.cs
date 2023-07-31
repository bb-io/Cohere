using Blackbird.Applications.Sdk.Common;

namespace Apps.Cohere.Models.Requests;

public class ClassifyTextWithFileExamplesRequest
{
    [Display("Text to classify")]
    public string Text { get; set; }
    
    [Display("Csv file with examples")]
    public byte[] CsvFileWithExamples { get; set; }
    
    public string Filename { get; set; }
    
    public string? Model { get; set; }
}