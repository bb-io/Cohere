using Blackbird.Applications.Sdk.Common;

namespace Apps.Cohere;

public class CohereApplication : IApplication
{
    public string Name
    {
        get => "Cohere";
        set { }
    }

    public T GetInstance<T>()
    {
        throw new NotImplementedException();
    }
}