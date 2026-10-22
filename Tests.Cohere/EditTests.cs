using Apps.Cohere.Actions;
using Apps.Cohere.Models.Requests;
using Blackbird.Filters.Enums;
using Tests.Cohere.Base;

namespace Tests.Cohere;

[TestClass]
public class EditTests : TestBase
{
    [TestMethod]
    public async Task EditText_IsSuccess()
    {
        var action = new EditActions(InvocationContext, FileManager);
        var corrected = await action.EditText(new EditTranslatedTextRequest
        {
            Model = ChatModel,
            SourceText = "The capital of France is Paris.",
            TargetText = "La capital de Francia es Londres.",
            SourceLanguage = "en",
            TargetLanguage = "es"
        });
        var untouched = await action.EditText(new EditTranslatedTextRequest
        {
            Model = ChatModel,
            SourceText = "Good morning.",
            TargetText = "Buenos días.",
            SourceLanguage = "en",
            TargetLanguage = "es"
        });

        Print(corrected);
        Print(untouched);
        StringAssert.Contains(corrected.EditedText, "París");
        Assert.AreEqual("Buenos días.", untouched.EditedText);
        AssertNoWarningsLogged();
    }

    [TestMethod]
    public async Task EditFile_IsSuccess()
    {
        var file = InMemoryFiles.Add("edit.xlf", InMemoryFileManager.Xliff("en", "it",
            ("The cat is sleeping on the sofa.", "Il cane dorme sul divano."),
            ("Good morning.", "Buongiorno.")));

        var action = new EditActions(InvocationContext, InMemoryFiles);
        var response = await action.EditFile(new EditFileRequest { File = file, Model = ChatModel, TargetLanguage = "it" });

        Print(response);
        var segments = InMemoryFiles.Segments(response.File);
        segments.ForEach(segment => Console.WriteLine($"{segment.GetSource()} => {segment.GetTarget()}"));
        Assert.AreEqual(2, response.TotalSegmentsReviewed);
        StringAssert.Contains(segments[0].GetTarget(), "gatto");
        Assert.AreEqual("Buongiorno.", segments[1].GetTarget());
        Assert.IsTrue(segments.All(segment => segment.State == SegmentState.Reviewed));
        AssertNoWarningsLogged();
    }
}
