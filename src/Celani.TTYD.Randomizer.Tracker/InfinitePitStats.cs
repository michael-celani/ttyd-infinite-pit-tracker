using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace Celani.TTYD.Randomizer.Tracker
{
    public class InfinitePitStats
    {
        [JsonIgnore]
        public Memory<byte> Data { get; }

        [JsonPropertyName("play_stats")]
        public InfinitePitPlayStats PlayStats { get; }

        [JsonPropertyName("last_save_time")]
        public ulong LastSaveTime => MemoryMarshal.AsRef<ulong>(Data.Span[112..120]);

        [JsonPropertyName("pit_start_time")]
        public ulong PitStartTime => MemoryMarshal.AsRef<ulong>(Data.Span[120..128]);

        [JsonPropertyName("star_power_levels")]
        private ushort StarPowerLevelsBitfield => MemoryMarshal.AsRef<ushort>(Data.Span[190..192]);

        [JsonPropertyName("star_power_levels")]
        public Dictionary<string, int> StarPowerLevels
        {
            get
            {
                var bitField = StarPowerLevelsBitfield;

                return new Dictionary<string, int>
                {
                    ["sweet_treat"] = bitField & 3,
                    ["earth_tremor"] = bitField >> 2 & 3,
                    ["clock_out"] = bitField >> 4 & 3,
                    ["power_lift"] = bitField >> 6 & 3,
                    ["art_attack"] = bitField >> 8 & 3,
                    ["sweet_feast"] = bitField >> 10 & 3,
                    ["showstopper"] = bitField >> 12 & 3,
                    ["supernova"] = bitField >> 14 & 3,
                };
            }
        }

        [JsonPropertyName("floor")]
        public uint Floor => MemoryMarshal.AsRef<uint>(Data.Span[196..200]);

        public InfinitePitStats(Memory<byte> data)
        {
            if (data.Length != 208)
            {
                throw new ArgumentException("Data must be 208 bytes long.", nameof(data));
            }

            Data = data;
            PlayStats = new(Data[0..64]);
        }
    }

    public class InfinitePitPlayStats
    {
        [JsonIgnore]
        public Memory<byte> Data { get; }

        [JsonPropertyName("total_turns")]
        public uint TotalTurns => ReadThree(Data.Span[61..64]);

        [JsonPropertyName("maximum_turns")]
        public ushort MaximumTurns => MemoryMarshal.AsRef<ushort>(Data.Span[59..61]);

        [JsonPropertyName("current_turns")]
        public ushort CurrentTurns => MemoryMarshal.AsRef<ushort>(Data.Span[57..59]);

        [JsonPropertyName("maximum_turns_floor")]
        public uint MaximumTurnsFloor => MemoryMarshal.AsRef<uint>(Data.Span[53..57]);

        [JsonPropertyName("times_ran_away")]
        public ushort TimesRanAway => MemoryMarshal.AsRef<ushort>(Data.Span[51..53]);

        [JsonPropertyName("damage_dealt")]
        public uint EnemyDamage => ReadThree(Data.Span[48..51]);

        [JsonPropertyName("damage_received")]
        public uint PlayerDamage => ReadThree(Data.Span[45..48]);

        [JsonPropertyName("items_used")]
        public uint ItemsUsed => ReadThree(Data.Span[42..45]);

        [JsonPropertyName("coins_earned")]
        public uint CoinsEarned => ReadThree(Data.Span[39..42]);

        [JsonPropertyName("coins_spent")]
        public uint CoinsSpent => ReadThree(Data.Span[36..39]);

        [JsonPropertyName("fp_spent")]
        public uint FlowerPointsSpent => ReadThree(Data.Span[33..36]);

        [JsonPropertyName("sp_spent")]
        public uint StarPointsSpent => ReadThree(Data.Span[30..33]);

        [JsonPropertyName("superguards")]
        public uint Superguards => ReadThree(Data.Span[27..30]);

        [JsonPropertyName("items_sold")]
        public uint ItemsSold => ReadThree(Data.Span[24..27]);

        [JsonPropertyName("badges_sold")]
        public uint BadgesSold => ReadThree(Data.Span[21..24]);

        [JsonPropertyName("levels_sold")]
        public ushort LevelsSold => MemoryMarshal.AsRef<ushort>(Data.Span[19..21]);

        [JsonPropertyName("shine_sprites")]
        public ushort ShineSprites => MemoryMarshal.AsRef<ushort>(Data.Span[17..19]);

        [JsonPropertyName("conditions_met")]
        public uint ConditionsMet => ReadThree(Data.Span[14..17]);

        [JsonPropertyName("conditions_total")]
        public uint ConditionsTotal => ReadThree(Data.Span[11..14]);

        [JsonPropertyName("movers_used")]
        public ushort MoversUsed => MemoryMarshal.AsRef<ushort>(Data.Span[9..11]);

        [JsonPropertyName("battles_skipped")]
        public uint BattlesSkipped => ReadThree(Data.Span[6..9]);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint ReadThree(ReadOnlySpan<byte> buffer)
        {
            return (uint)(MemoryMarshal.AsRef<ushort>(buffer[0..2]) | (buffer[2] << 16));
        }

        public InfinitePitPlayStats(Memory<byte> data)
        {
            if (data.Length != 64)
            {
                throw new ArgumentException("Data must be 64 bytes long.", nameof(data));
            }
                
            Data = data;
        }
    }

    /// <summary>
    /// Represents additional data kept by Infinite Pit.
    /// </summary>
    public class InfinitePitStatsSlim
    {
        [JsonIgnore]
        public byte[] Data { get; } = new byte[Marshal.SizeOf<ModData>()];

        [JsonIgnore]
        public byte[] TimeData { get; } = new byte[Marshal.SizeOf<FinalTimeData>()];

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint ReadThree(ReadOnlySpan<byte> buffer)
        {
            return (uint)(MemoryMarshal.Read<ushort>(buffer[0..2]) | (buffer[2] << 16));
        }

        public InfinitePitStatsSlim() { }

        public InfinitePitStatsSlim(InfinitePitStatsSlim other) : this()
        {
            var dataSpan = Data.AsSpan();
            other.Data.CopyTo(dataSpan);
        }
    }
}
