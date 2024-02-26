using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Celani.TTYD.Randomizer.Tracker
{
    /// <summary>
    /// The structure of the PouchData as laid out in memory in TTYD.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PouchData
    {
        public Badges equipped_badges;
        public Badges badges;
        public StoredItems stored_items;
        public Items items;
        public KeyItems key_items;
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
        public ushort star_powers_obtained;
        public short level;
        public short rank;
        public float audience_level;
        public UnknownBuffer unk_07e;
        public short max_sp;
        public short current_sp;
        public short coins;
        public short max_fp;
        public short current_fp;
        public short max_hp;
        public short current_hp;
        public PouchPartyData party_data;
    }

    [InlineArray(200)]
    public struct Badges
    {
        public short value;
    }

    [InlineArray(32)]
    public struct StoredItems
    {
        public short value;
    }

    [InlineArray(20)]
    public struct Items
    {
        public short value;
    }

    [InlineArray(121)]
    public struct KeyItems
    {
        public short value;
    }

    [InlineArray(6)]
    public struct UnknownBuffer
    {
        public byte value;
    }

    [InlineArray(8)]
    public struct PouchPartyData
    {
        public PouchPartyMember value;
    }

    /// <summary>
    /// The structure of a party member as laid out in memory in TTYD.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 14)]
    public struct PouchPartyMember
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
