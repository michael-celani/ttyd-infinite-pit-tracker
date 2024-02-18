using System.Runtime.InteropServices;

namespace Celani.TTYD.Randomizer.Tracker.Windows
{
    internal static partial class NativeFunctions
    {
        [LibraryImport("kernel32.dll")]
        internal static partial int VirtualQueryEx(nint hProcess, nint lpAddress, out MemoryBasicInformation lpBuffer, uint dwLength);
    }
}
