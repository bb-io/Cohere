using System.Globalization;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Cohere.DataSourceHandlers;

public class LocaleDataSourceHandler : IDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData(DataSourceContext context) =>
        CultureInfo.GetCultures(CultureTypes.SpecificCultures)
            .Where(culture => string.IsNullOrEmpty(context.SearchString)
                || culture.DisplayName.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Select(culture => new DataSourceItem(culture.Name, culture.DisplayName));
}
