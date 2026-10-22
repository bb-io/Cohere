using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Tests.Cohere.Base;
public class TestBase
{
    protected const string ChatModel = "command-a-03-2025";

    public IEnumerable<AuthenticationCredentialsProvider> Creds { get; set; }

    public InvocationContext InvocationContext { get; set; }

    public FileManager FileManager { get; set; }

    protected InMemoryFileManager InMemoryFiles { get; } = new();

    protected List<string> LoggedWarnings { get; } = [];

    public TestBase()
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        Creds = config.GetSection("ConnectionDefinition").GetChildren()
            .Select(x => new AuthenticationCredentialsProvider(x.Key, x.Value)).ToList();

        InvocationContext = new InvocationContext
        {
            AuthenticationCredentialsProviders = Creds,
            Logger = new Logger("Debug") { LogWarning = (message, _) => LoggedWarnings.Add(message ?? string.Empty) }
        };

        FileManager = new FileManager();
    }

    protected void AssertNoWarningsLogged() =>
        Assert.AreEqual(0, LoggedWarnings.Count, string.Join(Environment.NewLine, LoggedWarnings));

    protected static void Print(object result) =>
        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
}
