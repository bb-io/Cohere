using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Cohere.DataSourceHandlers;

public class SummaryExtractivenessDataSourceHandler : BaseInvocable, IDataSourceHandler
{
    public SummaryExtractivenessDataSourceHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public Dictionary<string, string> GetData(DataSourceContext context)
    {
        var summaryExtractivenesses = new[] { "Low", "Medium", "High", "Auto" }
            .Where(e => context.SearchString == null || e.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .ToDictionary(e => e, e => e);
        
        return summaryExtractivenesses;
    }
}