using System.Runtime.InteropServices;

namespace Celani.TTYD.Randomizer.Tracker
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ModData
    {
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 64)]
        public byte[] play_stats;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 32)]
        public byte[] option_bytes;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U4, SizeConst = 4)]
        public uint[] option_flags;

        public ulong last_save_time;
        public ulong pit_start_time;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U2, SizeConst = 28)]
        public ushort[] rng_sequences;

        public uint filename_seed;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 2)]
        public byte[] padding;

        public ushort star_power_levels;
        public uint reward_flags;

        public int floor;
        
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 7)]
        public byte[] partner_upgrades;

        public byte version; 
    }

}
