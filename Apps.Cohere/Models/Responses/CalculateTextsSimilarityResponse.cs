using Blackbird.Applications.Sdk.Common;

namespace Apps.Cohere.Models.Responses;

public class CalculateTextsSimilarityResponse
{
    [Display("Similarity score")]
    public decimal SimilarityScore { get; set; } 
}