using Apps.Cohere.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.SDK.Blueprints.Handlers;
using System.Text.Json.Serialization;

namespace Apps.Cohere.Models.Requests
{
    public class CohereTranslateFileRequest
    {
        [Display("File")]
        public FileReference File { get; set; }

        [Display("Source language")]
        [StaticDataSource(typeof(TranslateLanguagesDataHandler))]
        public string? SourceLanguage { get; set; }

        [Display("Target language")]
        [StaticDataSource(typeof(TranslateLanguagesDataHandler))]
        public string? TargetLanguage { get; set; }

        [Display("Preserve formatting")]
        public bool? PreserveFormatting { get; set; }

        [Display("Tone")]
        public string? Tone { get; set; }

        [Display("Instructions")]
        public string? Instructions { get; set; }

        [Display("Max tokens")]
        public int? MaxTokens { get; set; }

        [Display("Output file handling"), StaticDataSource(typeof(ProcessFileFormatHandler))]
        public string? OutputFileHandling { get; set; }
    }
}
