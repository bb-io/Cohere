using Apps.Cohere.Extensions;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Cohere.DataSourceHandlers;

public class TopPDataSourceHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData() =>
            ArrayExtensions.GenerateFormattedFloatArray(0.0f, 1.0f, 0.1f)
                .Select(p => new DataSourceItem(p, p));
}