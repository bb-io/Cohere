using Blackbird.Applications.Sdk.Common;

namespace Apps.Cohere.Models.Responses;

public class RerankTextsResponse
{
    [Display("Reranked texts")]
    public string RerankedTexts { get; set; }
}