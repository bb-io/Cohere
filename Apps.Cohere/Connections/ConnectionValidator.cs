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
        var request = new CohereRequest("/chat", Method.Post, authProviders)
            .AddJsonBody(new
            {
                message = "Test",
                temperature = 0.3,
                max_tokens = 8,
                stream = false
            });

        try
        {
            await new CohereClient(authProviders).ExecuteWithErrorHandling<ExtractEntityFromTextResponseWrapper>(request);

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