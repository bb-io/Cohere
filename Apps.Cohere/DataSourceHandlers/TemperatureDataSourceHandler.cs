using Apps.Cohere.Extensions;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Cohere.DataSourceHandlers;

public class TemperatureDataSourceHandler : BaseInvocable, IDataSourceHandler
{
    public TemperatureDataSourceHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public Dictionary<string, string> GetData(DataSourceContext context)
    {
        var temperatures = ArrayExtensions.GenerateFormattedFloatArray(0.0f, 5.0f, 0.1f)
            .Where(t => context.SearchString == null || t.Contains(context.SearchString))
            .ToDictionary(t => t, t => t);

        return temperatures;
    }
}