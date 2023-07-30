using Newtonsoft.Json;

namespace Apps.Cohere.Extensions;

public class SerializationExtensions
{
    public static T DeserializeResponseContent<T>(string content)
    {
        var deserializedContent = JsonConvert.DeserializeObject<T>(content, new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            }
        );
        return deserializedContent;
    }
}