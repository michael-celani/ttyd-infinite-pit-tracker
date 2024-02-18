using System;

namespace Celani.TTYD.Randomizer.API.Models
{
    /// <summary>
    /// Information about a currently active Pit run.
    /// </summary>
    public class PitRun
    {
        public bool IsStarted { get; set; }

        public int CurrentFloor { get; set; }

        public DateTime? CurrentFloorStart { get; set; }
    }
}
