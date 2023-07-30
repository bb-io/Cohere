namespace Apps.Cohere.Models.Responses;

public class GenerateEmbeddingsResponse
{
    public IEnumerable<double> Embedding { get; set; }
}

public class GenerateEmbeddingsResponseWrapper
{
    public IEnumerable<double[]> Embeddings { get; set; }
}