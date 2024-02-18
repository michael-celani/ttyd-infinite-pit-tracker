using System.Text.Json.Serialization;
using System.Text.Json;
using System;

namespace Celani.TTYD.Randomizer.API.Converters
{
    public class TimeSpanConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteStringValue($@"{value:hh\:mm\:ss\.ff}");
        }
    }
}
