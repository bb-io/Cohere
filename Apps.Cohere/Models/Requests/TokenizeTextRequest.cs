namespace Apps.Cohere.Models.Requests;

public class TokenizeTextRequest
{
    public string Text { get; set; }
    public string? Model { get; set; }
}