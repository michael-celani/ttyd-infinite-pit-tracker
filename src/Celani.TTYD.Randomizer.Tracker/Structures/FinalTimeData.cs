using System.Runtime.InteropServices;

namespace Celani.TTYD.Randomizer.Tracker
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FinalTimeData
    {
        public byte pit_finished;
        public ulong pit_final_time;
    }
}
