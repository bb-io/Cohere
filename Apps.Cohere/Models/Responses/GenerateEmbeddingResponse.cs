namespace Apps.Cohere.Models.Responses;

public class GenerateEmbeddingResponse
{
    public IEnumerable<double> Embedding { get; set; }
}

public class GenerateEmbeddingResponseWrapper
{
    public IEnumerable<double[]> Embeddings { get; set; }
}