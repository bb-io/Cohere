namespace Apps.Cohere.Models.Requests;

public class PerformGrammarAndSpellingCheckRequest
{
    public string Text { get; set; }

    public string? Model { get; set; }
}