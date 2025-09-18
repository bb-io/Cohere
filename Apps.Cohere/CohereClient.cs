using Apps.Cohere.Dtos;
using Apps.Cohere.Extensions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Cohere;

public class CohereClient : BlackBirdRestClient
{
    public CohereClient() : base(new RestClientOptions { ThrowOnAnyError = false, BaseUrl = GetBaseUrl() }) { }

    private static Uri GetBaseUrl() => new("https://api.cohere.ai/v1");
    
 
    public virtual async Task<T> ExecuteWithErrorHandling<T>(RestRequest request)
    {
        string content = (await ExecuteWithErrorHandling(request)).Content;
        T val = JsonConvert.DeserializeObject<T>(content, JsonSettings);
        if (val == null)
        {
            throw new Exception($"Could not parse {content} to {typeof(T)}");
        }

        return val;
    }

    public virtual async Task<RestResponse> ExecuteWithErrorHandling(RestRequest request)
    {
        RestResponse restResponse = await ExecuteAsync(request);
        if (!restResponse.IsSuccessStatusCode)
        {
            throw ConfigureErrorException(restResponse);
        }

        return restResponse;
    }

    protected override Exception ConfigureErrorException(RestResponse response)
    {
        var error = JsonConvert.DeserializeObject(response.Content!)!;
        throw new PluginApplicationException(error.ToString() ?? response.Content);
    }
}