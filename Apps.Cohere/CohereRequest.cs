using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;

namespace Apps.Cohere;

public class CohereRequest : RestRequest
{
    public CohereRequest(string endpoint, Method method,
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders) : base(endpoint, method)
    {
        this.AddHeader("Authorization", authenticationCredentialsProviders.First(p => p.KeyName == "Authorization").Value);
    }
}