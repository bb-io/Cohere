using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Cohere.DataSourceHandlers;

public class SummaryFormatDataSourceHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData() => new List<DataSourceItem>
    {
        new("Paragraph", "Paragraph"),
        new("Bullets", "Bullets"),
        new("Auto", "Auto")
    };
}