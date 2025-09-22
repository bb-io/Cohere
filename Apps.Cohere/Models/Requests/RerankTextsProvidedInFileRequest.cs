using Apps.Cohere.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Cohere.Models.Requests;

public class RerankTextsProvidedInFileRequest
{
    public string Query { get; set; }

    [Display("Txt file with texts")]
    public FileReference TxtFileWithTexts { get; set; }

    [Display("Number of most relevant texts to include")]
    public int TopN { get; set; }

    [StaticDataSource(typeof(RerankModelDataSourceHandler))]
    public string? Model { get; set; }

    [Display("Minimum relevance score (from 0.0 to 1.0)")]
    [StaticDataSource(typeof(RelevanceScoreDataSourceHandler))]
    public float? MinimumRelevanceScore { get; set; }
}