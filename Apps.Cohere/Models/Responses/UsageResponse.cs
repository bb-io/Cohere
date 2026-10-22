using Blackbird.Applications.Sdk.Common;

namespace Apps.Cohere.Models.Responses;

public class UsageResponse
{
    [Display("Prompt tokens")]
    public int PromptTokens { get; set; }

    [Display("Completion tokens")]
    public int CompletionTokens { get; set; }

    [Display("Total tokens")]
    public int TotalTokens { get; set; }
}
