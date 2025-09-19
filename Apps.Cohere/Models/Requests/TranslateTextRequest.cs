using Apps.Cohere.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Cohere.Models.Requests
{
    public class TranslateTextRequest
    {
        [Display("Text to translate")]
        public string Text { get; set; }

        [Display("Target language")]
        [StaticDataSource(typeof(TranslateLanguagesDataHandler))]
        public string TargetLanguage { get; set; }

        [Display("Source language")]
        [StaticDataSource(typeof(TranslateLanguagesDataHandler))]
        public string? SourceLanguage { get; set; }

        [Display("Preserve formatting")]
        public bool? PreserveFormatting { get; set; }

        [Display("Maximum number of tokens")]
        public int? MaxTokens { get; set; }

        [DataSource(typeof(GenerateTextModelDataSourceHandler))]
        public string? Model { get; set; }
    }
}
