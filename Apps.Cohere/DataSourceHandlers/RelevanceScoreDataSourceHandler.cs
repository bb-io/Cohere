using Apps.Cohere.Extensions;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Cohere.DataSourceHandlers;

public class RelevanceScoreDataSourceHandler : BaseInvocable, IDataSourceHandler
{
    public RelevanceScoreDataSourceHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public Dictionary<string, string> GetData(DataSourceContext context)
    {
        var scores = ArrayExtensions.GenerateFormattedFloatArray(0.0f, 1.0f, 0.1f)
            .Where(s => context.SearchString == null || s.Contains(context.SearchString))
            .ToDictionary(s => s, s => s);

        return scores;
    }
}