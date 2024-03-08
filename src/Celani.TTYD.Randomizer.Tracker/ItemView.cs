using System.Runtime.InteropServices;
using Celani.TTYD.Randomizer.Tracker.Lookups;

namespace Celani.TTYD.Randomizer.Tracker
{
    public readonly ref struct ItemView(Span<byte> data)
    {
        public Span<byte> Data { get; } = data;

        public string this[int index] 
        { 
            get 
            {
                var startIndex = index * 2;
                var endIndex = startIndex + 2;
                ref var code = ref MemoryMarshal.AsRef<short>(Data[startIndex..endIndex]);
                return NameLookup.GetItemName(code);
            }
        }

        public int Count => Data.Length >> 1;
    }
}