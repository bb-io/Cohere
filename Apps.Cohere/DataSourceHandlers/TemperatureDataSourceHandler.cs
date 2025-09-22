using Apps.Cohere.Extensions;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Cohere.DataSourceHandlers;

public class TemperatureDataSourceHandler : IDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData(DataSourceContext context)
    {
        return ArrayExtensions
            .GenerateFormattedFloatArray(0.0f, 5.0f, 0.1f)
            .Select(t => new DataSourceItem(t, t));
    }
}