using Apps.Cohere.Actions;
using Apps.Cohere.Models.Requests;
using Blackbird.Filters.Enums;
using Tests.Cohere.Base;

namespace Tests.Cohere;

[TestClass]
public class ReviewTests : TestBase
{
    [TestMethod]
    public async Task ReviewText_IsSuccess()
    {
        var action = new ReviewActions(InvocationContext, FileManager);
        var good = await action.ReviewText(new ReviewTextRequest
        {
            Model = ChatModel,
            SourceText = "This is a sample text to be reviewed for quality and accuracy.",
            TargetText = "Ceci est un texte d'exemple à examiner pour la qualité et l'exactitude.",
            SourceLanguage = "en",
            TargetLanguage = "fr"
        });
        var bad = await action.ReviewText(new ReviewTextRequest
        {
            Model = ChatModel,
            SourceText = "Our office opens at nine.",
            TargetText = "La lune est faite de fromage vert.",
            SourceLanguage = "en",
            TargetLanguage = "fr"
        });

        Print(good);
        Print(bad);
        Assert.IsTrue(good.Score is >= 0 and <= 1 && bad.Score is >= 0 and <= 1);
        Assert.IsTrue(good.Score > bad.Score, $"good {good.Score} vs bad {bad.Score}");
        AssertNoWarningsLogged();
    }

    [TestMethod]
    public async Task Review_IsSuccess()
    {
        var file = InMemoryFiles.Add("review.xlf", InMemoryFileManager.Xliff("en", "it",
            ("Please restart the application.", "Riavvia l'applicazione."),
            ("Our office opens at nine.", "La luna è fatta di formaggio verde.")));

        var action = new ReviewActions(InvocationContext, InMemoryFiles);
        var response = await action.ReviewFile(new ReviewFileRequest { File = file, Model = ChatModel, TargetLanguage = "it", Threshold = 0.8 });

        Print(response);
        var segments = InMemoryFiles.Segments(response.File);
        Assert.AreEqual(2, response.TotalSegmentsProcessed);
        Assert.AreEqual(1, response.TotalSegmentsFinalized);
        Assert.AreEqual(1, response.TotalSegmentsUnderThreshhold);
        Assert.AreEqual(SegmentState.Final, segments[0].State);
        Assert.AreNotEqual(SegmentState.Final, segments[1].State);
        AssertNoWarningsLogged();
    }
}
