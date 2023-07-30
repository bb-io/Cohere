using Blackbird.Applications.Sdk.Common;

namespace Apps.Cohere.Models.Requests;

public class GenerateEmbeddingsRequest
{
    [Display("Text to embed")]
    public string Text { get; set; }
    
    public string? Model { get; set; }
}