using Blackbird.Applications.Sdk.Common.Dynamic;
using Tests.Cohere.Base;

namespace Tests.Cohere;

[TestClass]
public class DataHandlerTests : TestBase
{
    [TestMethod]
    public async Task GenerateTextModelDataSourceHandler_IssSuccess()
    {
        var handler = new Apps.Cohere.DataSourceHandlers.GenerateTextModelDataSourceHandler(InvocationContext);
        var result = handler.GetData(new DataSourceContext { SearchString = "" });

        foreach (var item in result)
        {
            Console.WriteLine($"{item.Key}: {item.Value}");
        }
        Assert.IsNotNull(result);
    }
}
