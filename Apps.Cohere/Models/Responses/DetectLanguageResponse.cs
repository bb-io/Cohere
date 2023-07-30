using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Cohere.Models.Responses;

public class DetectLanguageResponse
{
    [Display("Language code")]
    [JsonProperty("language_code")]
    public string LanguageCode { get; set; }
    
    [Display("Language name")]
    [JsonProperty("language_name")]
    public string LanguageName { get; set; }
}

public class DetectLanguageResponseWrapper
{
    public IEnumerable<DetectLanguageResponse> Results { get; set; }
}