namespace Celani.TTYD.Randomizer.Tracker.Windows
{
    internal struct MemoryBasicInformation
    {
        public nint BaseAddress;
        public nint AllocationBase;
        public AllocationProtectEnum AllocationProtect;
        public nint RegionSize;
        public StateEnum State;
        public AllocationProtectEnum Protect;
        public TypeEnum Type;
    }
}
