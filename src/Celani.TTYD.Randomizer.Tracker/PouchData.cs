using System.Runtime.InteropServices;

namespace Celani.TTYD.Randomizer.Tracker
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PouchData
    {
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I2, SizeConst = 200)]
        public short[] equipped_badges;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I2, SizeConst = 200)]
        public short[] badges;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I2, SizeConst = 32)]
        public short[] stored_items;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I2, SizeConst = 20)]
        public short[] items;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I2, SizeConst = 121)]
        public short[] key_items;

        public short power_bounce_record;
        public short shine_sprites;
        public short star_pieces;
        public byte hammer_level;
        public byte jump_level;
        public short star_points;
        public short total_bp;
        public short unallocated_bp;
        public short base_max_fp;
        public short base_max_hp;
        public ushort star_powers_obtained; // bitfield
        public short level;
        public short rank;
        public float audience_level;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 6)]
        public byte[] unk_07e;

        public short max_sp;
        public short current_sp;
        public short coins;
        public short max_fp;
        public short current_fp;
        public short max_hp;
        public short current_hp;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 8)]
        public PouchPartyData[] party_data;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PouchPartyData
    {
        public short tech_level;
        public short attack_level;
        public short hp_level;
        public short current_hp;
        public short base_max_hp;
        public short max_hp;
        public ushort flags;
    }
}
