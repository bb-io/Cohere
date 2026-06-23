using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Cohere.DataSourceHandlers;

public class TokenizeModelDataSourceHandler(InvocationContext invocationContext)
    : BaseModelDataSourceHandler(invocationContext)
{
    protected override string Endpoint => CohereModelEndpoints.Tokenize;
}
