using System.Runtime.InteropServices;

namespace Celani.TTYD.Randomizer.Tracker.Windows
{
    internal static class NativeFunctions
    {
        [DllImport("kernel32.dll")]
        internal static extern int VirtualQueryEx(nint hProcess, nint lpAddress, out MemoryBasicInformation lpBuffer, uint dwLength);
    }
}
