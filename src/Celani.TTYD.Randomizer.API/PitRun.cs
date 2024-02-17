using System;

namespace Celani.TTYD.Randomizer.API
{
    public class PitRun
    {
        public bool IsStarted { get; set; }

        public int CurrentFloor { get; set; }

        public DateTime? CurrentFloorStart { get; set; }
    }
}
