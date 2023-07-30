using Newtonsoft.Json;

namespace Apps.Cohere.Converters;

public class RerankedTextConverter : JsonConverter
{
    private class RerankedText
    {
        public string Text { get; set; }
    }
    
    public override bool CanWrite => false;
    public override bool CanConvert(Type objectType) => false;

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.StartObject)
        {
            var text = serializer.Deserialize<RerankedText>(reader);
            return text.Text;
        }

        return null;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}