namespace Apps.Cohere.Models.Requests;

public class DetokenizeTokensRequest
{
    public IEnumerable<int> Tokens { get; set; }
    public string? Model { get; set; }
}