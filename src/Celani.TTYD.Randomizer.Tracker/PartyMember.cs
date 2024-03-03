using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace Celani.TTYD.Randomizer.Tracker
{
    public class PartyMember(Memory<byte> Data)
    {
        [JsonPropertyName("tech_level")]
        public short TechLevel => MemoryMarshal.AsRef<short>(Data.Span[0..2]);

        [JsonPropertyName("attack_level")]
        public short AttackLevel => MemoryMarshal.AsRef<short>(Data.Span[2..4]);

        [JsonPropertyName("hp_level")]
        public short HpLevel => MemoryMarshal.AsRef<short>(Data.Span[4..6]);

        [JsonPropertyName("current_hp")]
        public short CurrentHp => MemoryMarshal.AsRef<short>(Data.Span[6..8]);

        [JsonPropertyName("base_max_hp")]
        public short BaseMaxHp => MemoryMarshal.AsRef<short>(Data.Span[8..10]);

        [JsonPropertyName("max_hp")]
        public short MaxHp => MemoryMarshal.AsRef<short>(Data.Span[10..12]);

        [JsonPropertyName("flags")]
        public ushort Flags => MemoryMarshal.AsRef<ushort>(Data.Span[12..14]);
    }

    /// <summary>
    /// Represents a party member.
    /// </summary>
    /// <param name="Data">Backing data for this party member.</param>
    public class PartyMemberSlim(Memory<byte> Data)
    {
        [JsonPropertyName("tech_level")]
        public short TechLevel => MemoryMarshal.AsRef<short>(Data.Span[0..2]);

        [JsonPropertyName("flags")]
        public ushort Flags => MemoryMarshal.AsRef<ushort>(Data.Span[12..14]);
    }
}
