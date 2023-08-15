using Apps.Cohere.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Cohere.Models.Requests;

public class TokenizeTextRequest
{
    public string Text { get; set; }
    
    [DataSource(typeof(TokenizeModelDataSourceHandler))]
    public string? Model { get; set; }
}