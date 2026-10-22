using Apps.Cohere.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Cohere.Models.Requests;

public class ShortenTextRequest
{
    [Display("Text")]
    public string Text { get; set; } = string.Empty;

    [Display("Maximum length", Description = "Maximum number of characters of the shortened text.")]
    public int MaximumLength { get; set; }

    [Display("Model", Description = "This parameter controls which Cohere model answers your request. By default, the default Cohere chat model.")]
    [DataSource(typeof(GenerateTextModelDataSourceHandler))]
    public string? Model { get; set; }

    [Display("Retries", Description = "Number of extra attempts when the text is still too long. Default value: 3")]
    public int? Retries { get; set; }

    [Display("Additional instructions", Description = "Specify additional instructions to apply when shortening.")]
    public string? AdditionalInstructions { get; set; }

    [Display("Glossary")]
    public FileReference? Glossary { get; set; }
}
