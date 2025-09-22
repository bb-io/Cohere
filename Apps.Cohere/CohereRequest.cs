using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using RestSharp;

namespace Apps.Cohere;

public class CohereRequest : BlackBirdRestRequest
{
    public CohereRequest(string resource, Method method, IEnumerable<AuthenticationCredentialsProvider> creds) :
     base(resource, method, creds)
    {
    }

    protected override void AddAuth(IEnumerable<AuthenticationCredentialsProvider> creds)
    {
        this.AddHeader("Authorization", $"{creds.First(p => p.KeyName == "Authorization").Value}");
    }
}