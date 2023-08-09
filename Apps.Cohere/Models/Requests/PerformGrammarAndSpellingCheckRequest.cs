using Apps.Cohere.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Cohere.Models.Requests;

public class PerformGrammarAndSpellingCheckRequest
{
    public string Text { get; set; }

    [DataSource(typeof(GenerateTextModelDataSourceHandler))]
    public string? Model { get; set; }
}