using System.Text.Json.Serialization;
using Celani.TTYD.Randomizer.Tracker.Converters;

namespace Celani.TTYD.Randomizer.Tracker
{
    public class FloorSnapshot
    {
        [JsonPropertyName("floor")]
        public required int Floor { get; init; }

        [JsonPropertyName("pouch")]
        public required byte[] FloorEndPouch { get; init; }

        [JsonPropertyName("mod_data")]
        public required byte[] FloorEndStats { get; init; }

        [JsonPropertyName("duration")]
        [JsonConverter(typeof(TimeSpanConverter))]
        public required TimeSpan FloorDuration { get; init; }
    }
}
