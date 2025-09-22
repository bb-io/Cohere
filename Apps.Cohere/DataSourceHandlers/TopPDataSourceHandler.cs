using Apps.Cohere.Extensions;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Cohere.DataSourceHandlers;

public class TopPDataSourceHandler : IDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData(DataSourceContext context)
    {
        return ArrayExtensions
            .GenerateFormattedFloatArray(0.0f, 1.0f, 0.1f)
            .Select(p => new DataSourceItem(p, p));
    }
}