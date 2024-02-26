using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Celani.TTYD.Randomizer.Tracker
{
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
