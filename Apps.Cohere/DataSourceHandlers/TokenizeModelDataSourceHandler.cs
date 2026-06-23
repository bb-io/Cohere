using Apps.Cohere.Dtos;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Cohere.DataSourceHandlers;

public class TokenizeModelDataSourceHandler(InvocationContext invocationContext)
    : BaseModelDataSourceHandler(invocationContext)
{
    private static readonly string[] SupportedEndpoints =
    [
        CohereModelEndpoints.Chat,
        "generate",
        "summarize"
    ];

    protected override bool ShouldIncludeModel(CohereModelDto model)
        => model.Endpoints.Any(endpoint => SupportedEndpoints.Contains(endpoint, StringComparer.OrdinalIgnoreCase));
}
