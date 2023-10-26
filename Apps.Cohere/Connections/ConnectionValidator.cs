using Apps.Cohere.Models.Responses;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;
using RestSharp;

namespace Apps.Cohere.Connections;

public class ConnectionValidator : IConnectionValidator
{
    public async ValueTask<ConnectionValidationResponse> ValidateConnection(
        IEnumerable<AuthenticationCredentialsProvider> authProviders, CancellationToken cancellationToken)
    {
        var client = new CohereClient();
        var request = new CohereRequest("/generate", Method.Post, authProviders)
            .AddJsonBody(new
            {
                Prompt = "Test",
                Model = "command",
                Max_tokens = 100,
                Temperature = 0.75
            });

        try
        {
            await client.ExecuteWithHandling<ExtractEntityFromTextResponseWrapper>(request);

            return new()
            {
                IsValid = true
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                IsValid = false,
                Message = ex.Message
            };
        }
    }
}