using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.SDK.Blueprints.Interfaces.Edit;

namespace Apps.Cohere.Models.Responses;

public class EditFileResponse : IEditFileOutput
{
    public FileReference File { get; set; } = new();

    [Display("Usage")]
    public UsageResponse Usage { get; set; } = new();

    [Display("Total segments reviewed")]
    public int TotalSegmentsReviewed { get; set; }

    [Display("Total segments updated")]
    public int TotalSegmentsUpdated { get; set; }
}
