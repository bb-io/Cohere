using Blackbird.Applications.Sdk.Common;

namespace Apps.Cohere.Models.Responses;

public class GenerateTextResponse
{
    [Display("Generated text")]
    public string Text { get; set; }
}

public class GenerateTextResponseWrapper
{
    public IEnumerable<GenerateTextResponse> Generations { get; set; }
}