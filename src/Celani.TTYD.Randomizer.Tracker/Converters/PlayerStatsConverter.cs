using System.Text.Json.Serialization;
using System.Text.Json;

namespace Celani.TTYD.Randomizer.Tracker.Converters
{
    public class PlayerStatsConverter : JsonConverter<byte[]>
    {
        public override byte[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options)
        {
            var stats = new PlayerPouch(value);

            writer.WriteStartObject();

            WriteItemView(writer, "equipped_badges", stats.EquippedBadges);
            WriteItemView(writer, "badges", stats.Badges);
            WriteItemView(writer, "stored_items", stats.StoredItems);
            WriteItemView(writer, "items", stats.Items);
            WriteItemView(writer, "key_items", stats.KeyItems);
            writer.WriteNumber("power_bounce_record", stats.PowerBounceRecord);
            writer.WriteNumber("shine_sprites", stats.ShineSprites);
            writer.WriteNumber("star_pieces", stats.StarPieces);
            writer.WriteNumber("hammer_level", stats.HammerLevel);
            writer.WriteNumber("jump_level", stats.JumpLevel);
            writer.WriteNumber("star_points", stats.StarPoints);
            writer.WriteNumber("total_bp", stats.TotalBadgePoints);
            writer.WriteNumber("unallocated_bp", stats.UnallocatedBadgePoints);
            writer.WriteNumber("base_max_fp", stats.BaseMaxFlowerPoints);
            writer.WriteNumber("base_max_hp", stats.BaseMaxHitPoints);
            writer.WriteNumber("star_powers_obtained", stats.StarPowersObtained);
            writer.WriteNumber("level", stats.Level);
            writer.WriteNumber("rank", stats.Rank);
            writer.WriteNumber("audience_level", stats.AudienceLevel);
            writer.WriteNumber("max_sp", stats.MaxStarPoints);
            writer.WriteNumber("current_sp", stats.CurrentStarPoints);
            writer.WriteNumber("coins", stats.Coins);
            writer.WriteNumber("max_fp", stats.MaxFlowerPoints);
            writer.WriteNumber("current_fp", stats.CurrentFlowerPoints);
            writer.WriteNumber("max_hp", stats.MaxHitPoints);
            writer.WriteNumber("current_hp", stats.CurrentHitPoints);
            
            writer.WriteStartObject("party");
            WritePartyMember(writer, "goombella", stats.Party.Goombella);
            WritePartyMember(writer, "koops", stats.Party.Koops);
            WritePartyMember(writer, "flurrie", stats.Party.Flurrie);
            WritePartyMember(writer, "yoshi", stats.Party.Yoshi);
            WritePartyMember(writer, "vivian", stats.Party.Vivian);
            WritePartyMember(writer, "bobbery", stats.Party.Bobbery);
            WritePartyMember(writer, "ms_mowz", stats.Party.Mowz);
            writer.WriteEndObject();

            writer.WriteEndObject();
        }

        private static void WritePartyMember(Utf8JsonWriter writer, string name, PartyMember party)
        {
            var flags = party.Flags;

            if ((flags & 1) != 1)
            {
                return;
            }

            writer.WriteStartObject(name);
            writer.WriteNumber("tech_level", party.TechLevel);
            writer.WriteNumber("attack_level", party.AttackLevel);
            writer.WriteNumber("hp_level", party.HpLevel);
            writer.WriteNumber("current_hp", party.CurrentHp);
            writer.WriteNumber("base_max_hp", party.BaseMaxHp);
            writer.WriteNumber("max_hp", party.MaxHp);
            writer.WriteNumber("flags", party.Flags);
            writer.WriteEndObject();
        }

        private static void WriteItemView(Utf8JsonWriter writer, string name, ItemView view)
        {
            writer.WriteStartArray(name);

            for (var i = 0; i < view.Count; i++)
            {
                if (view[i] != string.Empty)
                {
                    writer.WriteStringValue(view[i]);
                }
            }

            writer.WriteEndArray();
        }   
    }
}
