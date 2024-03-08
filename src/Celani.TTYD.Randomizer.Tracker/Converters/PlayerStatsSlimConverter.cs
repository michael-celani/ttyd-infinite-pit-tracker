using System.Text.Json.Serialization;
using System.Text.Json;

namespace Celani.TTYD.Randomizer.Tracker.Converters
{
    public class PlayerStatsSlimConverter : JsonConverter<byte[]>
    {
        public override byte[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options)
        {
            var stats = new PlayerPouch(value);

            writer.WriteStartObject();

            writer.WriteNumber("hammer_level", stats.HammerLevel);
            writer.WriteNumber("jump_level", stats.JumpLevel);
            writer.WriteNumber("star_points", stats.StarPoints);
            writer.WriteNumber("total_bp", stats.TotalBadgePoints);
            writer.WriteNumber("base_max_fp", stats.BaseMaxFlowerPoints);
            writer.WriteNumber("base_max_hp", stats.BaseMaxHitPoints);
            writer.WriteNumber("level", stats.Level);
            writer.WriteNumber("coins", stats.Coins);
            writer.WriteNumber("max_fp", stats.MaxFlowerPoints);
            writer.WriteNumber("current_fp", stats.CurrentFlowerPoints);
            writer.WriteNumber("max_hp", stats.MaxHitPoints);
            writer.WriteNumber("current_hp", stats.CurrentHitPoints);
            
            writer.WritePropertyName("party_data");
            writer.WriteStartArray();
            WritePartyMember(writer, stats.Party.Mowz);
            WritePartyMember(writer, stats.Party.Vivian);
            WritePartyMember(writer, stats.Party.Flurrie);
            WritePartyMember(writer, stats.Party.Yoshi);
            WritePartyMember(writer, stats.Party.Bobbery);
            WritePartyMember(writer, stats.Party.Koops);
            WritePartyMember(writer, stats.Party.Goombella);
            writer.WriteEndArray();

            writer.WriteEndObject();
        }

        private static void WritePartyMember(Utf8JsonWriter writer, PartyMember party)
        {
            writer.WriteStartObject();
            writer.WriteNumber("tech_level", party.TechLevel);
            writer.WriteNumber("flags", party.Flags);
            writer.WriteEndObject();
        }
    }
}
