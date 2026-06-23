using Apps.Cohere.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Tests.Cohere.Base;

namespace Tests.Cohere;

[TestClass]
public class DataHandlerTests : TestBase
{
    [TestMethod]
    public async Task GenerateTextModelDataSourceHandler_IsSuccess()
    {
        var handler = new GenerateTextModelDataSourceHandler(InvocationContext);
        var result = await handler.GetDataAsync(new DataSourceContext(), CancellationToken.None);

        PrintResult(result);
        AssertDataSource(result);
    }

    [TestMethod]
    public async Task EmbedModelDataSourceHandler_IsSuccess()
    {
        var handler = new EmbedModelDataSourceHandler(InvocationContext);
        var result = await handler.GetDataAsync(new DataSourceContext(), CancellationToken.None);

        PrintResult(result);
        AssertDataSource(result);
    }

    [TestMethod]
    public async Task RerankModelDataSourceHandler_IsSuccess()
    {
        var handler = new RerankModelDataSourceHandler(InvocationContext);
        var result = await handler.GetDataAsync(new DataSourceContext(), CancellationToken.None);

        PrintResult(result);
        AssertDataSource(result);
    }

    [TestMethod]
    public async Task TokenizeModelDataSourceHandler_IsSuccess()
    {
        var handler = new TokenizeModelDataSourceHandler(InvocationContext);
        var result = await handler.GetDataAsync(new DataSourceContext(), CancellationToken.None);

        PrintResult(result);
        AssertDataSource(result);
    }

    [TestMethod]
    public async Task GenerateTextModelDataSourceHandler_Search_IsSuccess()
    {
        var handler = new GenerateTextModelDataSourceHandler(InvocationContext);
        var result = await handler.GetDataAsync(new DataSourceContext { SearchString = "command" }, CancellationToken.None);

        PrintResult(result);
        AssertDataSource(result);
        Assert.IsTrue(result.All(x => x.Value.Contains("command", StringComparison.OrdinalIgnoreCase)));
    }

    private static void PrintResult(IEnumerable<DataSourceItem> result)
    {
        foreach (var item in result)
        {
            Console.WriteLine($"{item.DisplayName}: {item.Value}");
        }
    }

    private static void AssertDataSource(IEnumerable<DataSourceItem> result)
    {
        Assert.AreNotEqual(0, result.Count());
        Assert.AreEqual(result.Select(x => x.Value).Count(), result.Select(x => x.Value).Distinct().Count());
    }
}
