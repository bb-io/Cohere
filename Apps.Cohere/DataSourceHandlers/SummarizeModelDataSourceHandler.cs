using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Cohere.DataSourceHandlers;

public class SummarizeModelDataSourceHandler : BaseInvocable, IDataSourceHandler
{
    public SummarizeModelDataSourceHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public Dictionary<string, string> GetData(DataSourceContext context)
    {
        var summarizeModels = new List<string>
        {
            "summarize-medium",
            "summarize-xlarge"
        };
        
        return summarizeModels
            .Where(m => context.SearchString == null || m.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .ToDictionary(m => m, m => m);
    }
}