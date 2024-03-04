using Celani.TTYD.Randomizer.Tracker.Lookups;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace Celani.TTYD.Randomizer.Tracker
{
    public class PlayerPouch
    {
        /// <summary>
        /// The backing store for the pouch.
        /// </summary>
        [JsonIgnore]
        public Memory<byte> Data { get; }

        [JsonPropertyName("equipped_badges")]
        public ItemView EquippedBadges { get; }

        [JsonPropertyName("badges")]
        public ItemView Badges { get; }

        [JsonPropertyName("stored_items")]
        public ItemView StoredItems { get; }

        [JsonPropertyName("items")]
        public ItemView Items { get; }

        [JsonPropertyName("key_items")]
        public ItemView KeyItems { get; }

        [JsonPropertyName("power_bounce_record")]
        public short PowerBounceRecord => MemoryMarshal.AsRef<short>(Data.Span[1146..1148]);

        [JsonPropertyName("shine_sprites")]
        public short ShineSprites => MemoryMarshal.AsRef<short>(Data.Span[1148..1150]);

        [JsonPropertyName("star_pieces")]
        public short StarPieces => MemoryMarshal.AsRef<short>(Data.Span[1150..1152]);

        [JsonPropertyName("hammer_level")]
        public byte HammerLevel => Data.Span[1152];

        [JsonPropertyName("jump_level")]
        public byte JumpLevel => Data.Span[1153];

        [JsonPropertyName("star_points")]
        public short StarPoints => MemoryMarshal.AsRef<short>(Data.Span[1154..1156]);

        [JsonPropertyName("total_bp")]
        public short TotalBadgePoints => MemoryMarshal.AsRef<short>(Data.Span[1156..1158]);

        [JsonPropertyName("unallocated_bp")]
        public short UnallocatedBadgePoints => MemoryMarshal.AsRef<short>(Data.Span[1158..1160]);

        [JsonPropertyName("base_max_fp")]
        public short BaseMaxFlowerPoints => MemoryMarshal.AsRef<short>(Data.Span[1160..1162]);

        [JsonPropertyName("base_max_hp")]
        public short BaseMaxHitPoints => MemoryMarshal.AsRef<short>(Data.Span[1162..1164]);

        [JsonPropertyName("star_powers_obtained")]
        public ushort StarPowersObtained => MemoryMarshal.AsRef<ushort>(Data.Span[1164..1166]);

        [JsonPropertyName("level")]
        public short Level => MemoryMarshal.AsRef<short>(Data.Span[1166..1168]);

        [JsonPropertyName("rank")]
        public short Rank => MemoryMarshal.AsRef<short>(Data.Span[1168..1170]);

        [JsonPropertyName("audience_level")]
        public float AudienceLevel => MemoryMarshal.AsRef<float>(Data.Span[1170..1174]);

        [JsonPropertyName("max_sp")]
        public short MaxStarPoints => MemoryMarshal.AsRef<short>(Data.Span[1180..1182]);

        [JsonPropertyName("current_sp")]
        public short CurrentStarPoints => MemoryMarshal.AsRef<short>(Data.Span[1182..1184]);

        [JsonPropertyName("coins")]
        public short Coins => MemoryMarshal.AsRef<short>(Data.Span[1184..1186]);

        [JsonPropertyName("max_fp")]
        public short MaxFlowerPoints => MemoryMarshal.AsRef<short>(Data.Span[1186..1188]);

        [JsonPropertyName("current_fp")]
        public short CurrentFlowerPoints => MemoryMarshal.AsRef<short>(Data.Span[1188..1190]);

        [JsonPropertyName("max_hp")]
        public short MaxHitPoints => MemoryMarshal.AsRef<short>(Data.Span[1190..1192]);

        [JsonPropertyName("current_hp")]
        public short CurrentHitPoints => MemoryMarshal.AsRef<short>(Data.Span[1192..1194]);

        [JsonPropertyName("party")]
        private Dictionary<string, PartyMember> Party { get; }

        [JsonPropertyName("party")]
        public Dictionary<string, PartyMember> UnlockedMembers => Party.Where(kvp => (kvp.Value.Flags & 1) == 1).ToDictionary();

        public PlayerPouch(Memory<byte> data)
        {
            if (data.Length != 1306)
            {
                throw new ArgumentException("Data must be 1306 bytes long.", nameof(data));
            }

            Data = data;
            EquippedBadges = new(Data[0..400]);
            Badges = new(Data[400..800]);
            StoredItems = new(Data[800..864]);
            Items = new(Data[864..904]);
            KeyItems = new(Data[904..1146]);

            Party = new() {
                ["goombella"] = new PartyMember(Data[1278..1292]),
                ["koops"] = new PartyMember(Data[1264..1278]),
                ["flurrie"] = new PartyMember(Data[1222..1236]),
                ["yoshi"] = new PartyMember(Data[1236..1250]),
                ["vivian"] = new PartyMember(Data[1208..1222]),
                ["bobbery"] = new PartyMember(Data[1250..1264]),
                ["mowz"] = new PartyMember(Data[1194..1208]),
            };
        }
    }

    public class ItemView(Memory<byte> data) : IReadOnlyList<string>
    {
        public Memory<byte> Data { get; } = data;

        public string this[int index] 
        { 
            get 
            {
                var span = Data.Span;
                var startIndex = index * 2;
                var endIndex = startIndex + 2;
                ref var code = ref MemoryMarshal.AsRef<short>(span[startIndex..endIndex]);
                return NameLookup.GetItemName(code);
            }
        }

        public int Count => Data.Length >> 1;

        public IEnumerator<string> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
            {
                var value = this[i];
                if (value != string.Empty) yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
            {
                var value = this[i];
                if (value != string.Empty) yield return this[i];
            }
        }
    }

    /// <summary>
    /// Represents Mario's party and inventory.
    /// </summary>
    public class PlayerStatsSlim
    {
        /// <summary>
        /// The backing store for the pouch.
        /// </summary>
        public byte[] Data { get; } = new byte[Marshal.SizeOf<PouchData>()];

        [JsonPropertyName("party_data")]
        public PartyMemberSlim[] Party { get; } = new PartyMemberSlim[8];

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ref PouchData GetPouchData() => ref MemoryMarshal.AsRef<PouchData>(Data);

        public PlayerStatsSlim()
        {
            var pouchSize = Marshal.SizeOf<PouchData>();
            var partySize = Marshal.SizeOf<PouchPartyMember>();
            var partyStart = pouchSize - partySize * 8;
            var memory = Data.AsMemory();

            for (var i = 0; i < 8; i++)
            {
                var memorySlice = memory.Slice(partyStart, partySize);
                Party[i] = new PartyMemberSlim(memorySlice);
                partyStart += partySize;
            }
        }

        public PlayerStatsSlim(PlayerStatsSlim other) : this()
        {
            var dataSpan = Data.AsSpan();
            other.Data.CopyTo(dataSpan);
        }
    }
}
