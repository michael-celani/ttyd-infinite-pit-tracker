using Celani.TTYD.Randomizer.Tracker;
namespace Celani.TTYD.Randomizer.API
{
    public class SentData
    {
        public string FileName { get; set; }

        public PouchData PouchData { get; set; }

        public ModData ModData { get; set; }

        public string PitRunStart { get; set; }

        public string PitRunElapsed { get; set; }
    }
}
