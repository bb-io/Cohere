using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Cohere.DataSourceHandlers;

public class RerankModelDataSourceHandler : BaseInvocable, IDataSourceHandler
{
    public RerankModelDataSourceHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public Dictionary<string, string> GetData(DataSourceContext context)
    {
        var rerankModels = new List<string>
        {
            "rerank-english-v2.0",
            "rerank-multilingual-v2.0"
        };
        
        return rerankModels
            .Where(m => context.SearchString == null || m.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .ToDictionary(m => m, m => m);
    }
}