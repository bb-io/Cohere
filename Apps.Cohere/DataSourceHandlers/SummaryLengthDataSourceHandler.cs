using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Cohere.DataSourceHandlers;

public class SummaryLengthDataSourceHandler : BaseInvocable, IDataSourceHandler
{
    public SummaryLengthDataSourceHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public Dictionary<string, string> GetData(DataSourceContext context)
    {
        var summaryLengths = new[] { "Short", "Medium", "Long", "Auto" }
            .Where(l => context.SearchString == null || l.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .ToDictionary(l => l, l => l);
        
        return summaryLengths;
    }
}