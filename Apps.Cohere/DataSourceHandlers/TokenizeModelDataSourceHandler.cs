using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Cohere.DataSourceHandlers;

public class TokenizeModelDataSourceHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData() => new List<DataSourceItem>
    {
        new("command-a-03-2025", "command-a-03-2025"),
        new("command-r7b-12-2024", "command-r7b-12-2024"),
        new("command-r-08-2024", "command-r-08-2024"),
        new("command-r-plus-08-2024", "command-r-plus-08-2024"),
        new("command-a-translate-08-2025", "command-a-translate-08-2025"),
        new("command-a-reasoning-08-2025", "command-a-reasoning-08-2025"),
        new("command-a-vision-07-2025", "command-a-vision-07-2025"),
        new("summarize-medium", "summarize-medium"),
        new("summarize-xlarge", "summarize-xlarge"),
    };
}