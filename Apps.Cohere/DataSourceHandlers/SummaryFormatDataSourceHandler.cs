using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Cohere.DataSourceHandlers;

public class SummaryFormatDataSourceHandler : BaseInvocable, IDataSourceHandler
{
    public SummaryFormatDataSourceHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public Dictionary<string, string> GetData(DataSourceContext context)
    {
        var summaryFormats = new[] { "Paragraph", "Bullets", "Auto" }
            .Where(f => context.SearchString == null || f.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .ToDictionary(f => f, f => f);
        
        return summaryFormats;
    }
}