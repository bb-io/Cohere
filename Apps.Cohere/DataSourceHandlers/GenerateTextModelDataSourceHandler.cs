using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Cohere.DataSourceHandlers;

public class GenerateTextModelDataSourceHandler : BaseInvocable, IDataSourceHandler
{
    public GenerateTextModelDataSourceHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public Dictionary<string, string> GetData(DataSourceContext context)
    {
        var generateTextModels = new List<string>
        {
            "command-a-03-2025",
            "command-r7b-12-2024",
            "command-r-08-2024",
            "command-r-plus-08-2024",
            "command-a-translate-08-2025",
            "command-a-reasoning-08-2025",
            "command-a-vision-07-2025"
        };

        return generateTextModels
            .Where(m => context.SearchString == null || m.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .ToDictionary(m => m, m => m);
    }
}