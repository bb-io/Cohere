using Blackbird.Applications.Sdk.Common;

namespace Apps.Cohere.Models.Responses;

public class AnalyzeTextResponse
{
    [Display("Analysis result")]
    public string Text { get; set; }
}

public class AnalyzeTextResponseWrapper
{
    public IEnumerable<AnalyzeTextResponse> Generations { get; set; }
}