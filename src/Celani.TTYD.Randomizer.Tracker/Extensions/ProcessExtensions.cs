using Celani.TTYD.Randomizer.Tracker.Windows;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Celani.TTYD.Randomizer.Tracker.Extensions
{
    internal static class ProcessExtensions
    {
        internal static IEnumerable<MemoryBasicInformation> EnumerateVirtualMemory(this Process process)
        {
            uint memoryBasicSize = (uint) Marshal.SizeOf<MemoryBasicInformation>();

            for (nint curAddress = 0; NativeFunctions.VirtualQueryEx(process.Handle, curAddress, out MemoryBasicInformation information, memoryBasicSize) == memoryBasicSize; curAddress += information.RegionSize)
            {
                yield return information;
            }
        }
    }
}
