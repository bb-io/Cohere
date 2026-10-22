using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.SDK.Blueprints.Interfaces.Review;

namespace Apps.Cohere.Models.Responses;

public class ReviewFileResponse : IReviewFileOutput
{
    public FileReference File { get; set; } = new();

    [Display("Usage")]
    public UsageResponse Usage { get; set; } = new();

    [Display("Total segments processed")]
    public int TotalSegmentsProcessed { get; set; }

    [Display("Total segments finalized")]
    public int TotalSegmentsFinalized { get; set; }

    [Display("Total segments under threshold")]
    public int TotalSegmentsUnderThreshhold { get; set; }

    [Display("Average metric")]
    public float AverageMetric { get; set; }

    [Display("Percentage segments under threshold")]
    public float PercentageSegmentsUnderThreshhold { get; set; }
}
