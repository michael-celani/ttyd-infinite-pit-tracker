using System.Text.Json.Serialization;

namespace Celani.TTYD.Randomizer.Tracker
{
    public class PitLog
    {
        [JsonPropertyName("seed")]
        public string Seed { get; set; } = string.Empty;

        [JsonPropertyName("floors")]
        public List<FloorSnapshot> FloorSnapshots { get; set; } = [];
    }
}
