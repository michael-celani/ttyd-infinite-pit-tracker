using Celani.TTYD.Randomizer.API.Converters;
using Celani.TTYD.Randomizer.Tracker;
using System;
using System.Text.Json.Serialization;

namespace Celani.TTYD.Randomizer.API.Models
{
    public class SentData(PitRun run)
    {
        public string FileName => run.Data.FileName;

        public PlayerStatsSlim PouchData => run.Data.Pouch;

        public InfinitePitStatsSlim ModData => run.Data.ModInfo;

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan PitRunElapsed => run.GetRunElapsed();

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan FloorRunElapsed => run.GetFloorElapsed();
    }
}
