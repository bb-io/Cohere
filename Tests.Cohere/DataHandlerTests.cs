using Apps.Cohere.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Tests.Cohere.Base;

namespace Tests.Cohere;

[TestClass]
public class DataHandlerTests : TestBase
{
    [TestMethod]
    public async Task GenerateTextModelDataSourceHandler_IssSuccess()
    {
        var handler = new GenerateTextModelDataSourceHandler();
        var result = handler.GetData();

        foreach (var item in result)
        {
            Console.WriteLine($"{item.DisplayName}: {item.Value}");
        }
        Assert.IsNotNull(result);
    }
}
