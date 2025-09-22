using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Cohere.DataSourceHandlers;

public class SummaryExtractivenessDataSourceHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData() => new List<DataSourceItem>
    {
        new("Low", "Low"),
        new("Medium", "Medium"),
        new("High", "High"),
        new("Auto", "Auto")
    };
}