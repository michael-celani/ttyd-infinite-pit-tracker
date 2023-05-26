namespace Celani.TTYD.Randomizer.Tracker.Windows
{
    [Flags]
    public enum StateEnum : uint
    {
        MEM_COMMIT = 0x1000,
        MEM_FREE = 0x10000,
        MEM_RESERVE = 0x2000
    }
}
