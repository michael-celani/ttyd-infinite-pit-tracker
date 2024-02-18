using Celani.TTYD.Randomizer.API.Converters;
using Celani.TTYD.Randomizer.Tracker;
using System;
using System.Text.Json.Serialization;

namespace Celani.TTYD.Randomizer.API.Models
{
    public class SentData
    {
        public string FileName { get; set; }

        public Pouch PouchData { get; set; }

        public InfinitePit ModData { get; set; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan PitRunElapsed { get; set; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan FloorRunElapsed { get; set; }
    }
}
