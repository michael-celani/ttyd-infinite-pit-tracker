using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace Celani.TTYD.Randomizer.Tracker
{
    public readonly ref struct Party(Span<byte> data)
    {
        private Span<byte> Data { get; } = data;

        public readonly PartyMember Mowz => new(Data[0..14]);

        public readonly PartyMember Vivian => new(Data[14..28]);

        public readonly PartyMember Flurrie => new(Data[28..42]);

        public readonly PartyMember Yoshi => new(Data[42..56]);

        public readonly PartyMember Bobbery => new(Data[56..70]);

        public readonly PartyMember Koops => new(Data[70..84]);

        public readonly PartyMember Goombella => new(Data[84..98]);
    }

    public readonly ref struct PartyMember(Span<byte> data)
    {
        private Span<byte> Data { get; } = data;

        [JsonPropertyName("tech_level")]
        public readonly ref short TechLevel => ref MemoryMarshal.AsRef<short>(Data[0..2]);

        [JsonPropertyName("attack_level")]
        public readonly ref short AttackLevel => ref MemoryMarshal.AsRef<short>(Data[2..4]);

        [JsonPropertyName("hp_level")]
        public readonly ref short HpLevel => ref MemoryMarshal.AsRef<short>(Data[4..6]);

        [JsonPropertyName("current_hp")]
        public readonly ref short CurrentHp => ref MemoryMarshal.AsRef<short>(Data[6..8]);

        [JsonPropertyName("base_max_hp")]
        public readonly ref short BaseMaxHp => ref MemoryMarshal.AsRef<short>(Data[8..10]);

        [JsonPropertyName("max_hp")]
        public readonly ref short MaxHp => ref MemoryMarshal.AsRef<short>(Data[10..12]);

        [JsonPropertyName("flags")]
        public readonly ref ushort Flags => ref MemoryMarshal.AsRef<ushort>(Data[12..14]);
    }
}
