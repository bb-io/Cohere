using Apps.Cohere.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.Cohere.Models.Requests;

public class RerankTextsProvidedInFileRequest
{
    public string Query { get; set; }
    
    [Display("Txt file with texts")]
    public File TxtFileWithTexts { get; set; }

    [Display("Number of most relevant texts to include")]
    public int TopN { get; set; }
    
    [DataSource(typeof(RerankModelDataSourceHandler))]
    public string? Model { get; set; }

    [Display("Minimum relevance score (from 0.0 to 1.0)")]
    [DataSource(typeof(RelevanceScoreDataSourceHandler))]
    public float? MinimumRelevanceScore { get; set; }
}