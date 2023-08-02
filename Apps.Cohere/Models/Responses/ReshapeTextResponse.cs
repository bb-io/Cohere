using Blackbird.Applications.Sdk.Common;

namespace Apps.Cohere.Models.Responses;

public class ReshapeTextResponse
{
    [Display("Reshaped text")]
    public string Text { get; set; }
}

public class ReshapeTextResponseWrapper
{
    public IEnumerable<ReshapeTextResponse> Generations { get; set; }
}