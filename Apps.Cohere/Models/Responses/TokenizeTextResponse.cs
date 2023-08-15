using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Cohere.Models.Responses;

public class TokenizeTextResponse
{
    public IEnumerable<int> Tokens { get; set; }

    [Display("Token strings")]
    [JsonProperty("token_strings")]
    public IEnumerable<string> TokenStrings { get; set; }
}