using System.Text.Json.Serialization;
using System;
using System.Text.Json;
using Celani.TTYD.Randomizer.API.Models;

namespace Celani.TTYD.Randomizer.API.Converters
{
    public class PitRunConverter : JsonConverter<PitRun>
    {
        public override PitRun Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, PitRun value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("floors");
            writer.WriteStartArray();

            foreach (var snapshot in value.FloorSnapshots)
            {
                writer.WriteStartObject();
                writer.WriteNumber("floor", snapshot.Floor + 1);
                writer.WriteNumber("duration", snapshot.FloorDuration.TotalMilliseconds);
                writer.WriteBase64String("pouch", snapshot.FloorEndPouch.Data);
                writer.WriteBase64String("mod_data", snapshot.FloorEndStats.Data);
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }
}
