using Blackbird.Applications.Sdk.Common;

namespace Apps.Cohere.Models.Responses;

public class SummariseTextAnalysesResponse
{
    [Display("Summary")]
    public string Text { get; set; }
}

public class SummariseTextAnalysesResponseWrapper
{
    public IEnumerable<SummariseTextAnalysesResponse> Generations { get; set; }
}