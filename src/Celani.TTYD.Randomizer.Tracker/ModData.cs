using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace Celani.TTYD.Randomizer.Tracker
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ModData
    {
        private PlayStats play_stats;
        private OptionBytes option_bytes;
        private OptionFlags option_flags;
        private ulong last_save_time;
        private ulong pit_start_time;
        private RandomNumberGenerationSequences rng_sequences;
        private uint filename_seed;
        private PaddingBuffer padding;
        private ushort star_power_levels;
        private uint reward_flags;
        private int floor;
        private PartnerUpgrades partner_upgrades;
        private byte version;

        [JsonPropertyName("floor")]
        public int Floor => floor;

        [JsonPropertyName("star_power_levels")]
        public ushort StarPowerLevels => star_power_levels;

        [JsonPropertyName("pit_start_time")]
        public ulong PitStartTime => pit_start_time;

        public readonly uint TotalTurns => ReadThree(play_stats[61..64]);

        public readonly ushort MaximumTurns => MemoryMarshal.Read<ushort>(play_stats[59..61]);

        public readonly ushort CurrentTurns => MemoryMarshal.Read<ushort>(play_stats[57..59]);

        public readonly uint MaximumTurnsFloor => MemoryMarshal.Read<uint>(play_stats[53..57]);

        public readonly ushort TimesRanAway => MemoryMarshal.Read<ushort>(play_stats[51..53]);

        public readonly uint EnemyDamage => ReadThree(play_stats[48..51]);

        public readonly uint PlayerDamage => ReadThree(play_stats[45..48]);

        public readonly uint ItemsUsed => ReadThree(play_stats[42..45]);

        public readonly uint CoinsEarned => ReadThree(play_stats[39..42]);

        public readonly uint CoinsSpent => ReadThree(play_stats[36..39]);

        public readonly uint FlowerPointsSpent => ReadThree(play_stats[33..36]);

        public readonly uint StarPointsSpent => ReadThree(play_stats[30..33]);

        public readonly uint Superguards => ReadThree(play_stats[27..30]);

        public readonly uint ItemsSold => ReadThree(play_stats[24..27]);

        public readonly uint BadgesSold => ReadThree(play_stats[21..24]);

        public readonly ushort LevelsSold => MemoryMarshal.Read<ushort>(play_stats[19..21]);

        public readonly ushort ShineSprites => MemoryMarshal.Read<ushort>(play_stats[17..19]);

        public readonly uint ConditionsMet => ReadThree(play_stats[14..17]);

        public readonly uint ConditionsTotal => ReadThree(play_stats[11..14]);

        public readonly ushort MoversUsed => MemoryMarshal.Read<ushort>(play_stats[9..11]);

        public readonly uint BattlesSkipped => ReadThree(play_stats[6..9]);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint ReadThree(ReadOnlySpan<byte> buffer)
        {
            return (uint)(MemoryMarshal.Read<ushort>(buffer[0..2]) | (buffer[2] << 16));
        }
    }

    [InlineArray(64)]
    public struct PlayStats
    {
        public byte value;
    }

    [InlineArray(32)]
    public struct OptionBytes
    {
        public byte value;
    }

    [InlineArray(4)]
    public struct OptionFlags
    {
        public uint value;
    }

    [InlineArray(28)]
    public struct RandomNumberGenerationSequences
    {
        public ushort value;
    }

    [InlineArray(2)]
    public struct PaddingBuffer
    {
        public byte value;
    }

    [InlineArray(7)]
    public struct PartnerUpgrades
    {
        public byte value;

        [JsonPropertyName("value")]
        public byte Value => value;
    }
}
