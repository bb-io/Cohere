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

        [Display("System prompt")]
        public string SystemPrompt { get; set; } = string.Empty;

        [Display("User prompt")]
        public string UserPrompt { get; set; } = string.Empty;

        [Display("Usage")]
        public UsageResponse Usage { get; set; } = new();
    }
}
