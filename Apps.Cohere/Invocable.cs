using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Cohere;

public class Invocable : BaseInvocable
{
    protected AuthenticationCredentialsProvider[] Creds =>
        InvocationContext.AuthenticationCredentialsProviders.ToArray();

    protected CohereClient Client { get; }
    public Invocable(InvocationContext invocationContext) : base(invocationContext)
    {
        Client = new CohereClient(invocationContext.AuthenticationCredentialsProviders);
    }
}

