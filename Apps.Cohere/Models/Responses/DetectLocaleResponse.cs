using Blackbird.Applications.Sdk.Common;

namespace Apps.Cohere.Models.Responses;

public class DetectLocaleResponse
{
    [Display("Locale")]
    public string Text { get; set; }
}

public class DetectLocaleResponseWrapper
{
    public IEnumerable<DetectLocaleResponse> Generations { get; set; }
}