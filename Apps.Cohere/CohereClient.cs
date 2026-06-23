using Apps.Cohere.Dtos;
using Apps.Cohere.Extensions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Cohere;

public class CohereClient : BlackBirdRestClient
{
    private readonly AuthenticationCredentialsProvider[] _credentials;

    public CohereClient(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
        : base(new RestClientOptions { ThrowOnAnyError = false, BaseUrl = GetBaseUrl() })
    {
        _credentials = authenticationCredentialsProviders.ToArray();
    }

    private static Uri GetBaseUrl() => new("https://api.cohere.ai/v1");

    public async Task<IReadOnlyList<CohereModelDto>> ListModelsAsync(string? endpoint = null, CancellationToken cancellationToken = default)
    {
        var models = new List<CohereModelDto>();
        string? nextPageToken = null;

        do
        {
            var request = new CohereRequest("/models", Method.Get, _credentials);
            request.AddQueryParameter("page_size", "1000");

            if (!string.IsNullOrWhiteSpace(endpoint))
            {
                request.AddQueryParameter("endpoint", endpoint);
            }

            if (!string.IsNullOrWhiteSpace(nextPageToken))
            {
                request.AddQueryParameter("page_token", nextPageToken);
            }

            var response = await ExecuteWithErrorHandling<CohereModelsResponse>(request);
            models.AddRange(response.Models.Where(model => !model.IsDeprecated && !string.IsNullOrWhiteSpace(model.Name)));
            nextPageToken = response.NextPageToken;
        }
        while (!string.IsNullOrWhiteSpace(nextPageToken));

        return models
            .GroupBy(model => model.Name, StringComparer.OrdinalIgnoreCase)
            .Select(group => group.First())
            .ToList();
    }

    public async Task<string> ResolveDefaultModelAsync(string endpoint, CancellationToken cancellationToken = default)
    {
        var request = new CohereRequest("/models", Method.Get, _credentials);
        request.AddQueryParameter("endpoint", endpoint);
        request.AddQueryParameter("default_only", "true");
        request.AddQueryParameter("page_size", "1000");

        var response = await ExecuteWithErrorHandling<CohereModelsResponse>(request);
        var defaultModel = response.Models
            .FirstOrDefault(model => !model.IsDeprecated && !string.IsNullOrWhiteSpace(model.Name))
            ?.Name;

        if (!string.IsNullOrWhiteSpace(defaultModel))
        {
            return defaultModel;
        }

        var fallbackModel = (await ListModelsAsync(endpoint, cancellationToken)).FirstOrDefault()?.Name;
        if (!string.IsNullOrWhiteSpace(fallbackModel))
        {
            return fallbackModel;
        }

        throw new PluginMisconfigurationException(
            $"Couldn't find a compatible Cohere model for the '{endpoint}' endpoint. Please specify a model manually.");
    }

    public override async Task<T> ExecuteWithErrorHandling<T>(RestRequest request)
    {
        string content = (await ExecuteWithErrorHandling(request)).Content;
        T val = JsonConvert.DeserializeObject<T>(content, JsonSettings);
        if (val == null)
        {
            throw new Exception($"Could not parse {content} to {typeof(T)}");
        }

        return val;
    }

    public override async Task<RestResponse> ExecuteWithErrorHandling(RestRequest request)
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
