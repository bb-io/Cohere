using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.SDK.Blueprints.Interfaces.Translate;
using Newtonsoft.Json;

namespace Apps.Cohere.Models.Responses
{
    public class TranslateTextResponse : ITranslateTextOutput
    {
        [Display("Translation")]
        [JsonProperty("text")]
        public string TranslatedText { get; set; }
    }
}
