using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace Celani.TTYD.Randomizer.Tracker
{
    /// <summary>
    /// Represents additional data kept by Infinite Pit.
    /// </summary>
    public class InfinitePit
    {
        public byte[] Data { get; } = new byte[Marshal.SizeOf<ModData>()];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ref ModData GetModData() => ref MemoryMarshal.AsRef<ModData>(Data);

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
        private static uint ReadThree(ReadOnlySpan<byte> buffer)
        {
            return (uint)(MemoryMarshal.Read<ushort>(buffer[0..2]) | (buffer[2] << 16));
        }
    }

    /// <summary>
    /// Represents the Infinite Pit additional data as it is represented in memory.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ModData
    {
        public PlayStats play_stats;
        public OptionBytes option_bytes;
        public OptionFlags option_flags;
        public ulong last_save_time;
        public ulong pit_start_time;
        public RandomNumberGenerationSequences rng_sequences;
        public uint filename_seed;
        public PaddingBuffer padding;
        public ushort star_power_levels;
        public uint reward_flags;
        public int floor;
        public PartnerUpgrades partner_upgrades;
        public byte version;
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
    }
}
