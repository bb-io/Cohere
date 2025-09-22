using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Cohere.DataSourceHandlers;

public class SummaryLengthDataSourceHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData() => new List<DataSourceItem>
        {
            new("Short", "Short"),
            new("Medium", "Medium"),
            new("Long", "Long"),
            new("Auto", "Auto")
        };
}