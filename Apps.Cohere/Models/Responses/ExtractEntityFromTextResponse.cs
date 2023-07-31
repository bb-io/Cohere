using Blackbird.Applications.Sdk.Common;

namespace Apps.Cohere.Models.Responses;

public class ExtractEntityFromTextResponse
{
    [Display("Extracted text")]
    public string Text { get; set; }
}

public class ExtractEntityFromTextResponseWrapper
{
    public IEnumerable<ExtractEntityFromTextResponse> Generations { get; set; }
}