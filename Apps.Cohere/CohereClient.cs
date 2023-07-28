using RestSharp;

namespace Apps.Cohere;

public class CohereClient : RestClient
{
    public CohereClient() : base(new RestClientOptions { ThrowOnAnyError = true, BaseUrl = GetBaseUrl() }) { }

    private static Uri GetBaseUrl()
    {
        return new Uri("https://api.cohere.ai/v1");
    }
}