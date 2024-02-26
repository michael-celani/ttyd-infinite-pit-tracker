using Celani.TTYD.Randomizer.Tracker.Dolphin;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace Celani.TTYD.Randomizer.Tracker
{
    /// <summary>
    /// Represents additional data kept by Infinite Pit.
    /// </summary>
    public class InfinitePitStats
    {
        public byte[] Data { get; } = new byte[Marshal.SizeOf<ModData>()];

        private byte[] TimeData { get; } = new byte[Marshal.SizeOf<FinalTimeData>()];

        /// <summary>
        /// The address of the mod state in TTYD memory.
        /// </summary>
        private const long ModStateAddress = 0x80b56aa0;

        /// <summary>
        /// The address of the RTA final time.
        /// </summary>
        private const long FinalTimeAddress = 0x80b56538;

        [JsonPropertyName("floor")]
        public int Floor
        {
            get
            {
                ref var modData = ref GetModData();
                return modData.floor;
            }
        }

        [JsonPropertyName("star_power_levels")]
        public ushort StarPowerLevels
        {
            get
            {
                ref var modData = ref GetModData();
                return modData.star_power_levels;
            }
        }

        [JsonPropertyName("pit_start_time")]
        public ulong PitStartTime
        {
            get
            {
                ref var modData = ref GetModData();
                return modData.pit_start_time;
            }
        }

        [JsonPropertyName("pit_started")]
        public bool PitStarted
        {
            get
            {
                ref var modData = ref GetModData();
                return modData.pit_start_time != 0;
            }
        }

        [JsonPropertyName("pit_finished")]
        public bool PitFinished
        {
            get
            {
                ref var timeData = ref GetTimeData();
                return timeData.pit_finished == 1;
            }
        }

        [JsonPropertyName("pit_end_time")]
        public ulong PitEndTime
        {
            get
            {
                ref var timeData = ref GetTimeData();
                return timeData.pit_finished == 1 ? timeData.pit_final_time : 0;
            }
        }

        public uint TotalTurns
        {
            get
            {
                ref var modData = ref GetModData();
                return ReadThree(modData.play_stats[61..64]);
            }
        }

        public ushort MaximumTurns
        {
            get
            {
                ref var modData = ref GetModData();
                return MemoryMarshal.Read<ushort>(modData.play_stats[59..61]);
            }
        }

        public ushort CurrentTurns
        {
            get
            {
                ref var modData = ref GetModData();
                return MemoryMarshal.Read<ushort>(modData.play_stats[57..59]);
            }
        }

        public uint MaximumTurnsFloor
        {
            get
            {
                ref var modData = ref GetModData();
                return MemoryMarshal.Read<uint>(modData.play_stats[53..57]);
            }
        }

        public ushort TimesRanAway
        {
            get
            {
                ref var modData = ref GetModData();
                return MemoryMarshal.Read<ushort>(modData.play_stats[51..53]);
            }
        }

        public uint EnemyDamage
        {
            get
            {
                ref var modData = ref GetModData();
                return ReadThree(modData.play_stats[48..51]);
            }
        }

        public uint PlayerDamage
        {
            get
            {
                ref var modData = ref GetModData();
                return ReadThree(modData.play_stats[45..48]);
            }
        }

        public uint ItemsUsed
        {
            get
            {
                ref var modData = ref GetModData();
                return ReadThree(modData.play_stats[42..45]);
            }
        }

        public uint CoinsEarned
        {
            get
            {
                ref var modData = ref GetModData();
                return ReadThree(modData.play_stats[39..42]);
            }
        }

        public uint CoinsSpent
        {
            get
            {
                ref var modData = ref GetModData();
                return ReadThree(modData.play_stats[36..39]);
            }
        }

        public uint FlowerPointsSpent
        {
            get
            {
                ref var modData = ref GetModData();
                return ReadThree(modData.play_stats[33..36]);
            }
        }

        public uint StarPointsSpent
        {
            get
            {
                ref var modData = ref GetModData();
                return ReadThree(modData.play_stats[30..33]);
            }
        }

        public uint Superguards
        {
            get
            {
                ref var modData = ref GetModData();
                return ReadThree(modData.play_stats[27..30]);
            }
        }

        public uint ItemsSold
        {
            get
            {
                ref var modData = ref GetModData();
                return ReadThree(modData.play_stats[24..27]);
            }
        }

        public uint BadgesSold
        {
            get
            {
                ref var modData = ref GetModData();
                return ReadThree(modData.play_stats[21..24]);
            }
        }

        public ushort LevelsSold
        {
            get
            {
                ref var modData = ref GetModData();
                return MemoryMarshal.Read<ushort>(modData.play_stats[19..21]);
            }
        }

        public ushort ShineSprites
        {
            get
            {
                ref var modData = ref GetModData();
                return MemoryMarshal.Read<ushort>(modData.play_stats[17..19]);
            }
        }

        public uint ConditionsMet
        {
            get
            {
                ref var modData = ref GetModData();
                return ReadThree(modData.play_stats[14..17]);
            }
        }

        public uint ConditionsTotal
        {
            get
            {
                ref var modData = ref GetModData();
                return ReadThree(modData.play_stats[11..14]);
            }
        }

        public ushort MoversUsed
        {
            get
            {
                ref var modData = ref GetModData();
                return MemoryMarshal.Read<ushort>(modData.play_stats[9..11]);
            }
        }

        public ushort BattlesSkipped
        {
            get
            {
                ref var modData = ref GetModData();
                return MemoryMarshal.Read<ushort>(modData.play_stats[6..9]);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ref ModData GetModData() => ref MemoryMarshal.AsRef<ModData>(Data);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ref FinalTimeData GetTimeData() => ref MemoryMarshal.AsRef<FinalTimeData>(TimeData);

        public void Read(GamecubeGame game)
        {
            // Read the ModData.
            game.Read(ModStateAddress, Data);
            Data.AsSpan().Reverse();

            game.Read(FinalTimeAddress, TimeData);
            TimeData.AsSpan().Reverse();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint ReadThree(ReadOnlySpan<byte> buffer)
        {
            return (uint)(MemoryMarshal.Read<ushort>(buffer[0..2]) | (buffer[2] << 16));
        }

        public InfinitePitStats() { }

        public InfinitePitStats(InfinitePitStats other) : this()
        {
            var dataSpan = Data.AsSpan();
            other.Data.CopyTo(dataSpan);
        }
    }
}
