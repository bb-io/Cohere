using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Cohere.DataSourceHandlers;

public class EmbedModelDataSourceHandler : IStaticDataSourceItemHandler
{

    public IEnumerable<DataSourceItem> GetData() => new List<DataSourceItem>
    {
        new("embed-english-v3.0", "embed-english-v3.0"),
        new("embed-multilingual-v3.0", "embed-multilingual-v3.0"),

        new("embed-english-v2.0", "embed-english-v2.0 (legacy)"),
        new("embed-multilingual-v2.0", "embed-multilingual-v2.0 (legacy)"),
        new("embed-english-light-v2.0", "embed-english-light-v2.0 (legacy)")
    };
}