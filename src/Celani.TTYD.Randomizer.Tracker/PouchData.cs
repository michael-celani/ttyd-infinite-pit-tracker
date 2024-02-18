using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace Celani.TTYD.Randomizer.Tracker
{
    /// <summary>
    /// Represents Mario's party and inventory.
    /// </summary>
    public class Pouch
    {
        /// <summary>
        /// The backing store for the pouch.
        /// </summary>
        public byte[] Data { get; } = new byte[Marshal.SizeOf<PouchData>()];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ref PouchData GetPouchData() => ref MemoryMarshal.AsRef<PouchData>(Data);

        [JsonPropertyName("party_data")]
        public PartyMember[] Party { get; } = new PartyMember[8];

        public Pouch()
        {
            var pouchSize = Marshal.SizeOf<PouchData>();
            var partySize = Marshal.SizeOf<PouchPartyMember>();
            var partyStart = pouchSize - partySize * 8;

            for (var i = 0; i < 8; i++)
            {
                Party[i] = new PartyMember(Data.AsMemory()[partyStart..(partyStart + partySize)]);
                partyStart += partySize;
            }
        }

        [JsonPropertyName("coins")]
        public short Coins
        {
            get
            {
                ref var pouch = ref GetPouchData();
                return pouch.coins;
            }
        }

        [JsonPropertyName("star_points")]
        public short StarPoints
        {
            get
            {
                ref var pouch = ref GetPouchData();
                return pouch.star_points;
            }
        }

        [JsonPropertyName("level")]
        public short Level
        {
            get
            {
                ref var pouch = ref GetPouchData();
                return pouch.level;
            }
        }

        [JsonPropertyName("base_max_hp")]
        public short BaseMaxHp
        {
            get
            {
                ref var pouch = ref GetPouchData();
                return pouch.base_max_hp;
            }
        }

        [JsonPropertyName("base_max_fp")]
        public short BaseMaxFp
        {
            get
            {
                ref var pouch = ref GetPouchData();
                return pouch.base_max_fp;
            }
        }

        [JsonPropertyName("total_bp")]
        public short TotalBp
        {
            get
            {
                ref var pouch = ref GetPouchData();
                return pouch.total_bp;
            }
        }

        [JsonPropertyName("jump_level")]
        public short JumpLevel
        {
            get
            {
                ref var pouch = ref GetPouchData();
                return pouch.jump_level;
            }
        }

        [JsonPropertyName("hammer_level")]
        public short HammerLevel
        {
            get
            {
                ref var pouch = ref GetPouchData();
                return pouch.hammer_level;
            }
        }
    }

    /// <summary>
    /// Represents a party member.
    /// </summary>
    /// <param name="Data">Backing data for this party member.</param>
    public class PartyMember(Memory<byte> Data)
    {
        public ref PouchPartyMember GetPouchPartyMember() => ref MemoryMarshal.AsRef<PouchPartyMember>(Data.Span);

        [JsonPropertyName("flags")]
        public ushort Flags
        {
            get
            {
                ref var pouch = ref GetPouchPartyMember();
                return pouch.flags;
            }
        }

        [JsonPropertyName("tech_level")]
        public short TechLevel
        {
            get
            {
                ref var pouch = ref GetPouchPartyMember();
                return pouch.tech_level;
            }
        }
    }

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
