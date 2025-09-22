using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Cohere.DataSourceHandlers;

public class RerankModelDataSourceHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData() => new List<DataSourceItem>
    {
        new("rerank-english-v3.0", "rerank-english-v3.0"),
        new("rerank-multilingual-v3.0", "rerank-multilingual-v3.0"),
        new("rerank-english-v2.0", "rerank-english-v2.0"),
        new("rerank-multilingual-v2.0", "rerank-multilingual-v2.0")
    };
}