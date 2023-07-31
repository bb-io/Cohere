using Blackbird.Applications.Sdk.Common;

namespace Apps.Cohere.Models.Responses;

public class EditTextResponse
{
    [Display("Edited text")]
    public string Text { get; set; }
}

public class EditTextResponseWrapper
{
    public IEnumerable<EditTextResponse> Generations { get; set; }
}