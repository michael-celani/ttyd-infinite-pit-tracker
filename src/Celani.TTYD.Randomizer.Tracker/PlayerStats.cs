using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace Celani.TTYD.Randomizer.Tracker
{
    public readonly ref struct PlayerPouch(Span<byte> data)
    {
        private Span<byte> Data { get; } = data;

        [JsonPropertyName("equipped_badges")]
        public readonly ItemView EquippedBadges => new(Data[0..400]);

        [JsonPropertyName("badges")]
        public readonly ItemView Badges => new(Data[400..800]);

        [JsonPropertyName("stored_items")]
        public readonly ItemView StoredItems => new(Data[800..864]);

        [JsonPropertyName("items")]
        public readonly ItemView Items => new(Data[864..904]);

        [JsonPropertyName("key_items")]
        public readonly ItemView KeyItems => new(Data[904..1146]);

        [JsonPropertyName("power_bounce_record")]
        public readonly ref short PowerBounceRecord => ref MemoryMarshal.AsRef<short>(Data[1146..1148]);

        [JsonPropertyName("shine_sprites")]
        public readonly ref short ShineSprites => ref MemoryMarshal.AsRef<short>(Data[1148..1150]);

        [JsonPropertyName("star_pieces")]
        public readonly ref short StarPieces => ref MemoryMarshal.AsRef<short>(Data[1150..1152]);

        [JsonPropertyName("hammer_level")]
        public readonly ref byte HammerLevel => ref Data[1152];

        [JsonPropertyName("jump_level")]
        public readonly ref byte JumpLevel => ref Data[1153];

        [JsonPropertyName("star_points")]
        public readonly ref short StarPoints => ref MemoryMarshal.AsRef<short>(Data[1154..1156]);

        [JsonPropertyName("total_bp")]
        public readonly ref short TotalBadgePoints => ref MemoryMarshal.AsRef<short>(Data[1156..1158]);

        [JsonPropertyName("unallocated_bp")]
        public readonly ref short UnallocatedBadgePoints => ref MemoryMarshal.AsRef<short>(Data[1158..1160]);

        [JsonPropertyName("base_max_fp")]
        public readonly ref short BaseMaxFlowerPoints => ref MemoryMarshal.AsRef<short>(Data[1160..1162]);

        [JsonPropertyName("base_max_hp")]
        public readonly ref short BaseMaxHitPoints => ref MemoryMarshal.AsRef<short>(Data[1162..1164]);

        [JsonPropertyName("star_powers_obtained")]
        public readonly ref ushort StarPowersObtained => ref MemoryMarshal.AsRef<ushort>(Data[1164..1166]);

        [JsonPropertyName("level")]
        public readonly ref short Level => ref MemoryMarshal.AsRef<short>(Data[1166..1168]);

        [JsonPropertyName("rank")]
        public readonly ref short Rank => ref MemoryMarshal.AsRef<short>(Data[1168..1170]);

        [JsonPropertyName("audience_level")]
        public readonly ref float AudienceLevel => ref MemoryMarshal.AsRef<float>(Data[1170..1174]);

        [JsonPropertyName("max_sp")]
        public readonly ref short MaxStarPoints => ref MemoryMarshal.AsRef<short>(Data[1180..1182]);

        [JsonPropertyName("current_sp")]
        public readonly ref short CurrentStarPoints => ref MemoryMarshal.AsRef<short>(Data[1182..1184]);

        [JsonPropertyName("coins")]
        public readonly ref short Coins => ref MemoryMarshal.AsRef<short>(Data[1184..1186]);

        [JsonPropertyName("max_fp")]
        public readonly ref short MaxFlowerPoints => ref MemoryMarshal.AsRef<short>(Data[1186..1188]);

        [JsonPropertyName("current_fp")]
        public readonly ref short CurrentFlowerPoints => ref MemoryMarshal.AsRef<short>(Data[1188..1190]);

        [JsonPropertyName("max_hp")]
        public readonly ref short MaxHitPoints => ref MemoryMarshal.AsRef<short>(Data[1190..1192]);

        [JsonPropertyName("current_hp")]
        public readonly ref short CurrentHitPoints => ref MemoryMarshal.AsRef<short>(Data[1192..1194]);

        [JsonPropertyName("party")]
        public readonly Party Party => new (Data[1194..1306]);
    }
}
