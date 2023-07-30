namespace Apps.Cohere.Models.Responses;

public class ClassifyTextsResponse
{
    public string Input { get; set; }
    public string Prediction { get; set; }
    public float Confidence { get; set; }
}

public class ClassifyTextsResponseWrapper
{
    public IEnumerable<ClassifyTextsResponse> Classifications { get; set; }
}