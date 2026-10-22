using Apps.Cohere.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.SDK.Blueprints.Interfaces.Review;

namespace Apps.Cohere.Models.Requests;

public class ReviewTextRequest : IReviewTextInput
{
    [Display("Source text")]
    public string SourceText { get; set; } = string.Empty;

    [Display("Target text")]
    public string TargetText { get; set; } = string.Empty;

    [Display("Source language")]
    [DataSource(typeof(LocaleDataSourceHandler))]
    public string SourceLanguage { get; set; } = string.Empty;

    [Display("Target language")]
    [DataSource(typeof(LocaleDataSourceHandler))]
    public string TargetLanguage { get; set; } = string.Empty;

    [Display("Model", Description = "This parameter controls which Cohere model answers your request. By default, the default Cohere chat model.")]
    [DataSource(typeof(GenerateTextModelDataSourceHandler))]
    public string? Model { get; set; }

    [Display("Additional instructions", Description = "Additional instructions applied to the review.\nFor example: 'Focus on technical terminology accuracy.'")]
    public string? AdditionalInstructions { get; set; }

    [Display("Max tokens", Description = "The maximum number of tokens to generate. By default, the model's output limit.")]
    public int? MaxTokens { get; set; }

    [Display("Temperature", Description = "Amount of randomness injected into the response.")]
    [DataSource(typeof(TemperatureDataSourceHandler))]
    public double? Temperature { get; set; }

    [Display("Top P", Description = "Use nucleus sampling.")]
    [DataSource(typeof(TopPDataSourceHandler))]
    public double? TopP { get; set; }

    [Display("Glossary")]
    public FileReference? Glossary { get; set; }
}
