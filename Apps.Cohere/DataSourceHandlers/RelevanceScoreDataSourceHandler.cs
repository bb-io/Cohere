using Apps.Cohere.Extensions;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Cohere.DataSourceHandlers;

public class RelevanceScoreDataSourceHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData() =>
            ArrayExtensions.GenerateFormattedFloatArray(0.0f, 1.0f, 0.1f)
                .Select(s => new DataSourceItem(s, s));
}