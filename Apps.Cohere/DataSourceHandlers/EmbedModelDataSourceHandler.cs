using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Cohere.DataSourceHandlers;

public class EmbedModelDataSourceHandler : BaseInvocable, IDataSourceHandler
{
    public EmbedModelDataSourceHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public Dictionary<string, string> GetData(DataSourceContext context)
    {
        var embedModels = new List<string>
        {
            "embed-english-light-v2.0",
            "embed-multilingual-v2.0",
            "embed-english-v2.0"
        };
        
        return embedModels
            .Where(m => context.SearchString == null || m.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .ToDictionary(m => m, m => m);
    }
}