using Apps.Cohere;
using Apps.Cohere.Ai;
using Blackbird.Ai.Llm.Models;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Tests.Cohere.Base;

namespace Tests.Cohere;

[TestClass]
public class LlmClientTests : TestBase
{
    [TestMethod]
    public async Task CompleteAsync_OutputLimitReached_ReportsTruncated()
    {
        var client = new CohereLlmClient(new CohereClient(Creds), ChatModel, new(MaxTokens: 5));

        var response = await client.CompleteAsync(
            new LlmRequest("You are terse.", "Count from 1 to 50, separated by spaces."),
            CancellationToken.None);

        Print(response);
        Assert.AreEqual(FinishReason.Truncated, response.Finish);
        Assert.AreEqual(5, client.Capabilities.MaxOutputTokens);
        Assert.IsTrue(response.Usage.InputTokens > 0 && response.Usage.OutputTokens > 0);
    }

    [TestMethod]
    public async Task CompleteAsync_UnknownModel_ThrowsReadableMessage()
    {
        var client = new CohereLlmClient(new CohereClient(Creds), "no-such-model", new());

        var exception = await Assert.ThrowsExceptionAsync<PluginApplicationException>(() =>
            client.CompleteAsync(new LlmRequest("You are terse.", "Say hi."), CancellationToken.None));

        Console.WriteLine(exception.Message);
        StringAssert.StartsWith(exception.Message, "model 'no-such-model' not found");
    }

    [TestMethod]
    public void Capabilities_SmallContextModel_LeavesRoomForInput()
    {
        var capabilities = CohereModelLimits.Capabilities("command-a-translate-08-2025", null);
        var unknown = CohereModelLimits.Capabilities("my-fine-tuned-model", null);

        Print(capabilities);
        Assert.AreEqual(4_496, capabilities.MaxOutputTokens);
        Assert.AreEqual(4_496, capabilities.ContextTokens);
        Assert.AreEqual(4_096, unknown.MaxOutputTokens);
        Assert.AreEqual(4_096, unknown.ContextTokens);
    }
}
