using Apps.Cohere.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.SDK.Blueprints.Interfaces.Translate;

namespace Apps.Cohere.Models.Requests
{
    public class TranslateTextRequest : ITranslateTextInput
    {
        [Display("Text to translate")]
        public string Text { get; set; }

        [Display("Target language")]
        [DataSource(typeof(LocaleDataSourceHandler))]
        public string TargetLanguage { get; set; }

        [Display("Source language", Description = "Optional. If not set, it is detected from the content.")]
        [DataSource(typeof(LocaleDataSourceHandler))]
        public string? SourceLanguage { get; set; }

        [Display("Maximum number of tokens", Description = "The maximum number of tokens to generate. By default, the model's output limit.")]
        public int? MaxTokens { get; set; }

        [Display("Model", Description = "This parameter controls which Cohere model answers your request. Default value: command-a-translate-08-2025")]
        [DataSource(typeof(GenerateTextModelDataSourceHandler))]
        public string? Model { get; set; }

        [Display("Additional instructions", Description = "Specify additional instructions to be applied to the translation")]
        public string? AdditionalInstructions { get; set; }

        [Display("Glossary")]
        public FileReference? Glossary { get; set; }
    }
}
