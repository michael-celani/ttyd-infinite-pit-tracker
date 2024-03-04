using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Celani.TTYD.Randomizer.API.Models
{
    public class PitLog
    {
        [JsonPropertyName("seed")]
        public string Seed { get; set; }

        [JsonPropertyName("floors")]
        public List<FloorSnapshot> FloorSnapshots { get; set; } = [];
    }
}
