using Apps.Cohere.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Cohere.Models.Requests;

public class GenerateEmbeddingRequest
{
    [Display("Text to embed")]
    public string Text { get; set; }

    [DataSource(typeof(EmbedModelDataSourceHandler))]
    public string? Model { get; set; }
}