using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace Celani.TTYD.Randomizer.Tracker
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PouchData
    {
        private Badges equipped_badges;
        private Badges badges;
        private StoredItems stored_items;
        private Items items;
        private KeyItems key_items;
        private short power_bounce_record;
        private short shine_sprites;
        private short star_pieces;
        private byte hammer_level;
        private byte jump_level;
        private short star_points;
        private short total_bp;
        private short unallocated_bp;
        private short base_max_fp;
        private short base_max_hp;
        private ushort star_powers_obtained; // bitfield
        private short level;
        private short rank;
        private float audience_level;
        private UnknownBuffer unk_07e;
        private short max_sp;
        private short current_sp;
        private short coins;
        private short max_fp;
        private short current_fp;
        private short max_hp;
        private short current_hp;
        private PouchPartyData party_data;

        [JsonPropertyName("coins")]
        public readonly short Coins => coins;

        [JsonPropertyName("star_points")]
        public readonly short StarPoints => star_points;

        [JsonPropertyName("level")]
        public readonly short Level => level;

        [JsonPropertyName("base_max_hp")]
        public readonly short BaseMaxHp => base_max_hp;

        [JsonPropertyName("base_max_fp")]
        public readonly short BaseMaxFp => base_max_fp;

        [JsonPropertyName("total_bp")]
        public readonly short TotalBp => total_bp;

        [JsonPropertyName("jump_level")]
        public readonly short JumpLevel => jump_level;

        [JsonPropertyName("hammer_level")]
        public readonly short HammerLevel => hammer_level;

        [JsonPropertyName("party_data")]
        public readonly ImmutableArray<PouchPartyMember> PartyData => party_data[0..8].ToImmutableArray();
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

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 14)]
    public struct PouchPartyMember
    {
        private short tech_level;
        private short attack_level;
        private short hp_level;
        private short current_hp;
        private short base_max_hp;
        private short max_hp;
        private ushort flags;

        [JsonPropertyName("flags")]
        public readonly ushort Flags => flags;

        [JsonPropertyName("tech_level")]
        public readonly short TechLevel => tech_level;
    }
}
