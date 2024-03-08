using System.Text.Json.Serialization;
using System.Text.Json;

namespace Celani.TTYD.Randomizer.Tracker.Converters
{
    public class TimeSpanConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                return TimeSpan.FromMilliseconds(reader.GetDouble());
            }

            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            var totalHours = value.TotalHours;

            if (totalHours <= 0)
            {
                writer.WriteStringValue("00:00:00.00");
                return;
            }

            if (totalHours >= 100)
            {
                writer.WriteStringValue("99:59:59.99");
                return;
            }

            Span<char> arr = stackalloc char[11];
            var hoursInt = (int) totalHours;
            hoursInt.TryFormat(arr[0..2], out var charsWritten, "00");
            value.TryFormat(arr[2..], out charsWritten, @"\:mm\:ss\.ff");
            writer.WriteStringValue(arr);
        }
    }
}
