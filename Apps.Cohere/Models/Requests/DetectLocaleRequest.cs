namespace Apps.Cohere.Models.Requests;

public class DetectLocaleRequest
{
    public string Text { get; set; }
    public string? Model { get; set; }
}