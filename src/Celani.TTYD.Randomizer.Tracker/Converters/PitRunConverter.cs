using System.Text.Json.Serialization;
using System.Text.Json;

namespace Celani.TTYD.Randomizer.Tracker.Converters
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
            writer.WriteString("seed", value.PitLog.Seed);

            writer.WriteStartArray("floors");
            foreach (var snapshot in value.PitLog.FloorSnapshots)
            {
                writer.WriteStartObject();
                writer.WriteNumber("floor", snapshot.Floor + 1);
                writer.WriteNumber("duration", snapshot.FloorDuration.TotalMilliseconds);
                writer.WriteBase64String("pouch", snapshot.FloorEndPouch);
                writer.WriteBase64String("mod_data", snapshot.FloorEndStats);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
            
            writer.WriteEndObject();
        }
    }
}
