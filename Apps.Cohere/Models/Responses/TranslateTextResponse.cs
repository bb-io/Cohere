using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.SDK.Blueprints.Interfaces.Translate;

namespace Apps.Cohere.Models.Responses
{
    public class TranslateTextResponse : ITranslateTextOutput
    {
        [Display("Translation")]
        public string TranslatedText { get; set; }
    }
}
