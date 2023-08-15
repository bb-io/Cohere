using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Cohere.DataSourceHandlers;

public class TokenizeModelDataSourceHandler : BaseInvocable, IDataSourceHandler
{
    public TokenizeModelDataSourceHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public Dictionary<string, string> GetData(DataSourceContext context)
    {
        var tokenizeModels = new List<string>
        {
            "base", 
            "base-light", 
            "command", 
            "command-light", 
            "command-light-nightly", 
            "command-nightly",
            "summarize-medium",
            "summarize-xlarge"
        };
        
        return tokenizeModels
            .Where(m => context.SearchString == null || m.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .ToDictionary(m => m, m => m);
    }
}