using Apps.Cohere.Dtos;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Cohere.DataSourceHandlers;

public abstract class BaseModelDataSourceHandler(InvocationContext invocationContext)
    : Invocable(invocationContext), IAsyncDataSourceItemHandler
{
    protected abstract string Endpoint { get; }

    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var models = await Client.ListModelsAsync(Endpoint, cancellationToken);
        var searchString = context.SearchString;

        return models
            .Where(model => searchString == null || model.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(IsDefaultForEndpoint)
            .ThenBy(model => model.Name, StringComparer.OrdinalIgnoreCase)
            .Select(model => new DataSourceItem(model.Name, model.Name));
    }

    private bool IsDefaultForEndpoint(CohereModelDto model)
        => model.DefaultEndpoints.Contains(Endpoint, StringComparer.OrdinalIgnoreCase);
}
