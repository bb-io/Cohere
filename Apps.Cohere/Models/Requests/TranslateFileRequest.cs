using Apps.Cohere.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.SDK.Blueprints.Handlers;
using Blackbird.Applications.SDK.Blueprints.Interfaces.Translate;

namespace Apps.Cohere.Models.Requests
{
    public class CohereTranslateFileRequest : ITranslateFileInput
    {
        [Display("File")]
        public FileReference File { get; set; }

        [Display("Source language", Description = "Optional. If not set, it is taken from the file or detected from the content.")]
        [DataSource(typeof(LocaleDataSourceHandler))]
        public string? SourceLanguage { get; set; }

        [Display("Target language", Description = "Optional if the file already declares its target language.")]
        [DataSource(typeof(LocaleDataSourceHandler))]
        public string? TargetLanguage { get; set; }

        [Display("Tone", Description = "The tone of the translation. For example: 'formal'")]
        public string? Tone { get; set; }

        [Display("Instructions", Description = "Specify additional instructions to be applied to the translation")]
        public string? Instructions { get; set; }

        [Display("Max tokens", Description = "The maximum number of tokens to generate per request. By default, the model's output limit.")]
        public int? MaxTokens { get; set; }

        [Display("Model", Description = "This parameter controls which Cohere model answers your request. Default value: command-a-translate-08-2025")]
        [DataSource(typeof(GenerateTextModelDataSourceHandler))]
        public string? Model { get; set; }

        [Display("Output file handling", Description = "Determine the format of the output file. The default Blackbird behavior is to convert to XLIFF for future steps.")]
        [StaticDataSource(typeof(ProcessFileFormatHandler))]
        public string? OutputFileHandling { get; set; }

        [Display("Bucket size", Description = "Specify the maximum number of source texts to be translated at once. Default value: 250. Buckets are also sized to fit the model's output limit. (See our documentation for an explanation)")]
        public int? BucketSize { get; set; }

        [Display("Generate document summary", Description = "By default, a short summary of the document is generated and given to the model as context for files of 20 or more segments. This costs one extra request. Set to false to skip it.")]
        public bool? GenerateDocumentSummary { get; set; }

        [Display("Glossary")]
        public FileReference? Glossary { get; set; }
    }
}
