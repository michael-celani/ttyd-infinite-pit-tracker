using Celani.TTYD.Randomizer.Tracker;
using System;

namespace Celani.TTYD.Randomizer.API.Models
{
    public class FloorSnapshot
    {
        public int Floor { get; init; }

        public PlayerStats FloorEndPouch { get; init; }

        public InfinitePitStats FloorEndStats { get; init; }

        public TimeSpan FloorDuration { get; init; }
    }
}
