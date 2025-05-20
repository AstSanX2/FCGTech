using MongoDB.Bson;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace FCG.API.Helpers
{
    public class ObjectIdJsonConverter : JsonConverter<ObjectId>
    {
        public override ObjectId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();

            if (string.IsNullOrEmpty(value))
            {
                return default;
            }

            if (ObjectId.TryParse(value, out var result))
            {
                return result;
            }

            return default;
        }

        public override void Write(Utf8JsonWriter writer, ObjectId value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
