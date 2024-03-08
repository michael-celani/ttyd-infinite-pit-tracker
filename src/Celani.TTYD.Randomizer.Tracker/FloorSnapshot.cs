using System.Text.Json.Serialization;
using Celani.TTYD.Randomizer.Tracker.Converters;

namespace Celani.TTYD.Randomizer.Tracker
{
    public class FloorSnapshot
    {
        [JsonPropertyName("floor")]
        public int Floor { get; init; }

        [JsonPropertyName("pouch")]
        public byte[] FloorEndPouch { get; init; }

        [JsonPropertyName("mod_data")]
        public byte[] FloorEndStats { get; init; }

        [JsonPropertyName("duration")]
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan FloorDuration { get; init; }
    }
}
