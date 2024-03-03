using Celani.TTYD.Randomizer.Tracker;
using System;

namespace Celani.TTYD.Randomizer.API.Models
{
    public class FloorSnapshot
    {
        public int Floor { get; init; }

        public PlayerStatsSlim FloorEndPouch { get; init; }

        public InfinitePitStatsSlim FloorEndStats { get; init; }

        public TimeSpan FloorDuration { get; init; }
    }
}
