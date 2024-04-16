using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Metadata;

namespace Apps.Cohere;

public class CohereApplication : IApplication, ICategoryProvider
{
    public IEnumerable<ApplicationCategory> Categories
    {
        get => [ApplicationCategory.ArtificialIntelligence];
        set { }
    }
    
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