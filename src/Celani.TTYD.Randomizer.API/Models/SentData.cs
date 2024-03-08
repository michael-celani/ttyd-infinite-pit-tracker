using Celani.TTYD.Randomizer.Tracker;
using Celani.TTYD.Randomizer.Tracker.Converters;
using System;
using System.Text.Json.Serialization;

namespace Celani.TTYD.Randomizer.API.Models
{
    public class SentData(PitRun run)
    {
        public string FileName => run.Data.FileName;

        [JsonConverter(typeof(PlayerStatsSlimConverter))]
        public byte[] PouchData => run.Data.Pouch;

        [JsonConverter(typeof(ModDataSlimConverter))]
        public byte[] ModData => run.Data.ModInfo;

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan PitRunElapsed => run.GetRunElapsed();

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan FloorRunElapsed => run.GetFloorElapsed();
    }
}
