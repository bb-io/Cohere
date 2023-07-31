using Blackbird.Applications.Sdk.Common;

namespace Apps.Cohere.Models.Responses;

public class PerformGrammarAndSpellingCheckResponse
{
    [Display("Corrected text")]
    public string Text { get; set; }
}

public class PerformGrammarAndSpellingCheckResponseWrapper
{
    public IEnumerable<PerformGrammarAndSpellingCheckResponse> Generations { get; set; }
}