using Apps.Cohere.Extensions;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Cohere.DataSourceHandlers;

public class TemperatureDataSourceHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData() =>
            ArrayExtensions.GenerateFormattedFloatArray(0.0f, 5.0f, 0.1f)
                .Select(t => new DataSourceItem(t, t));
}