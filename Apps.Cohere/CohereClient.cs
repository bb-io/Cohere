using Apps.Cohere.Dtos;
using Apps.Cohere.Extensions;
using RestSharp;

namespace Apps.Cohere;

public class CohereClient : RestClient
{
    public CohereClient() : base(new RestClientOptions { ThrowOnAnyError = false, BaseUrl = GetBaseUrl() }) { }

    private static Uri GetBaseUrl() => new("https://api.cohere.ai/v1");
    
    public async Task<T> ExecuteWithHandling<T>(RestRequest request)
    {
        var response = await ExecuteWithHandling(request);
        return SerializationExtensions.DeserializeResponseContent<T>(response.Content);
    }

    private async Task<RestResponse> ExecuteWithHandling(RestRequest request)
    {
        var response = await ExecuteAsync(request);
        
        if (response.IsSuccessful)
            return response;

        throw ConfigureErrorException(response.Content);
    }

    private Exception ConfigureErrorException(string responseContent)
    {
        var error = SerializationExtensions.DeserializeResponseContent<ErrorDto>(responseContent);
        return new(error.Message);
    }
}