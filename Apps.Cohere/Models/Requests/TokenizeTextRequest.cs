using Apps.Cohere.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Cohere.Models.Requests;

public class TokenizeTextRequest
{
    public string Text { get; set; }
    
    [StaticDataSource(typeof(TokenizeModelDataSourceHandler))]
    public string? Model { get; set; }
}