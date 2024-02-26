using Celani.TTYD.Randomizer.Tracker.Dolphin;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace Celani.TTYD.Randomizer.Tracker
{
    /// <summary>
    /// Represents Mario's party and inventory.
    /// </summary>
    public class PlayerStats
    {
        /// <summary>
        /// The backing store for the pouch.
        /// </summary>
        public byte[] Data { get; } = new byte[Marshal.SizeOf<PouchData>()];

        /// <summary>
        /// The address of the pouch in TTYD memory.
        /// </summary>
        private const long PouchAddress = 0x80b07b60;

        [JsonPropertyName("party_data")]
        public PartyMember[] Party { get; } = new PartyMember[8];

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

        public void Read(GamecubeGame game)
        {
            // Read the ModData.
            game.Read(PouchAddress, Data);
            Data.AsSpan().Reverse();
        }

        public PlayerStats()
        {
            var pouchSize = Marshal.SizeOf<PouchData>();
            var partySize = Marshal.SizeOf<PouchPartyMember>();
            var partyStart = pouchSize - partySize * 8;
            var memory = Data.AsMemory();

            for (var i = 0; i < 8; i++)
            {
                var memorySlice = memory.Slice(partyStart, partySize);
                Party[i] = new PartyMember(memorySlice);
                partyStart += partySize;
            }
        }

        public PlayerStats(PlayerStats other) : this()
        {
            var dataSpan = Data.AsSpan();
            other.Data.CopyTo(dataSpan);
        }
    }
}
