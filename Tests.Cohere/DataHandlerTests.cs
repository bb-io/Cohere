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

        foreach (var item in result)
        {
            Console.WriteLine($"{item.DisplayName}: {item.Value}");
        }

        Assert.AreNotEqual(0, result.Count());
    }
}
