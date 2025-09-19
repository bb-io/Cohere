using Blackbird.Applications.Sdk.Common;

namespace Apps.Cohere.Models.Responses
{
    public class TranslateTextResponse
    {
        [Display("Translation")]
        public string Text { get; set; }
    }
}
