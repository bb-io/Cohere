using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Cohere.DataSourceHandlers;

public class EmbedModelDataSourceHandler(InvocationContext invocationContext)
    : BaseModelDataSourceHandler(invocationContext)
{
    protected override string Endpoint => CohereModelEndpoints.Embed;
}
