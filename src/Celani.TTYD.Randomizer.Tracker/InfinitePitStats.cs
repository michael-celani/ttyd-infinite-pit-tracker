using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace Celani.TTYD.Randomizer.Tracker
{
    public readonly ref struct InfinitePitStats(Span<byte> data)
    {
        private Span<byte> Data { get; } = data;

        [JsonPropertyName("play_stats")]
        public readonly InfinitePitPlayStats PlayStats => new(Data[0..64]);

        [JsonPropertyName("last_save_time")]
        public readonly ref ulong LastSaveTime => ref MemoryMarshal.AsRef<ulong>(Data[112..120]);

        [JsonPropertyName("pit_start_time")]
        public readonly ref ulong PitStartTime => ref MemoryMarshal.AsRef<ulong>(Data[120..128]);

        [JsonPropertyName("star_power_levels_bitfield")]
        public readonly ref ushort StarPowerLevelsBitfield => ref MemoryMarshal.AsRef<ushort>(Data[190..192]);

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
        public readonly ref uint Floor => ref MemoryMarshal.AsRef<uint>(Data[196..200]);
    }

    public readonly ref struct InfinitePitPlayStats(Span<byte> data)
    {
        private Span<byte> Data { get; } = data;

        [JsonPropertyName("total_turns")]
        public uint TotalTurns => ReadThree(Data[61..64]);

        [JsonPropertyName("maximum_turns")]
        public ushort MaximumTurns => MemoryMarshal.AsRef<ushort>(Data[59..61]);

        [JsonPropertyName("current_turns")]
        public ushort CurrentTurns => MemoryMarshal.AsRef<ushort>(Data[57..59]);

        [JsonPropertyName("maximum_turns_floor")]
        public uint MaximumTurnsFloor => MemoryMarshal.AsRef<uint>(Data[53..57]);

        [JsonPropertyName("times_ran_away")]
        public ushort TimesRanAway => MemoryMarshal.AsRef<ushort>(Data[51..53]);

        [JsonPropertyName("damage_dealt")]
        public uint EnemyDamage => ReadThree(Data[48..51]);

        [JsonPropertyName("damage_received")]
        public uint PlayerDamage => ReadThree(Data[45..48]);

        [JsonPropertyName("items_used")]
        public uint ItemsUsed => ReadThree(Data[42..45]);

        [JsonPropertyName("coins_earned")]
        public uint CoinsEarned => ReadThree(Data[39..42]);

        [JsonPropertyName("coins_spent")]
        public uint CoinsSpent => ReadThree(Data[36..39]);

        [JsonPropertyName("fp_spent")]
        public uint FlowerPointsSpent => ReadThree(Data[33..36]);

        [JsonPropertyName("sp_spent")]
        public uint StarPointsSpent => ReadThree(Data[30..33]);

        [JsonPropertyName("superguards")]
        public uint Superguards => ReadThree(Data[27..30]);

        [JsonPropertyName("items_sold")]
        public uint ItemsSold => ReadThree(Data[24..27]);

        [JsonPropertyName("badges_sold")]
        public uint BadgesSold => ReadThree(Data[21..24]);

        [JsonPropertyName("levels_sold")]
        public ushort LevelsSold => MemoryMarshal.AsRef<ushort>(Data[19..21]);

        [JsonPropertyName("shine_sprites")]
        public ushort ShineSprites => MemoryMarshal.AsRef<ushort>(Data[17..19]);

        [JsonPropertyName("conditions_met")]
        public uint ConditionsMet => ReadThree(Data[14..17]);

        [JsonPropertyName("conditions_total")]
        public uint ConditionsTotal => ReadThree(Data[11..14]);

        [JsonPropertyName("movers_used")]
        public ushort MoversUsed => MemoryMarshal.AsRef<ushort>(Data[9..11]);

        [JsonPropertyName("battles_skipped")]
        public uint BattlesSkipped => ReadThree(Data[6..9]);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint ReadThree(ReadOnlySpan<byte> buffer)
        {
            return (uint)(MemoryMarshal.AsRef<ushort>(buffer[0..2]) | (buffer[2] << 16));
        }
    }

    public readonly ref struct InfinitePitFinalTime(Span<byte> data)
    {
        private Span<byte> Data { get; } = data;

        [JsonPropertyName("pit_finished")]
        public bool PitFinished => Data[0] != 0;

        [JsonPropertyName("pit_end_time")]
        public readonly ref ulong PitEndTime => ref MemoryMarshal.AsRef<ulong>(Data[1..9]);
    }
}
