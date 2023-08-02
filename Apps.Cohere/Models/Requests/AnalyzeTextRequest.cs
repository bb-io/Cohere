namespace Apps.Cohere.Models.Requests;

public class AnalyzeTextRequest
{
    public string Text { get; set; }
    
    public string? Model { get; set; }
}