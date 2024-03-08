using System.Text.Json.Serialization;
using System.Text.Json;

namespace Celani.TTYD.Randomizer.Tracker.Converters
{
    public class ModDataSlimConverter : JsonConverter<byte[]>
    {
        public override byte[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options)
        {
            var stats = new InfinitePitStats(value);

            writer.WriteStartObject();
            writer.WriteNumber("floor", stats.Floor);
            writer.WriteNumber("pit_start_time", stats.PitStartTime);
            writer.WriteNumber("star_power_levels", stats.StarPowerLevelsBitfield);
            WritePlayStats(writer, stats.PlayStats);
            writer.WriteEndObject();
        }

        private static void WritePlayStats(Utf8JsonWriter writer, InfinitePitPlayStats stats)
        {
            writer.WriteStartObject("play_stats");
            writer.WriteNumber("damage_dealt", stats.EnemyDamage);
            writer.WriteNumber("damage_received", stats.PlayerDamage);
            writer.WriteNumber("items_used", stats.ItemsUsed);
            writer.WriteNumber("coins_earned", stats.CoinsEarned);
            writer.WriteNumber("fp_spent", stats.FlowerPointsSpent);
            writer.WriteNumber("sp_spent", stats.StarPointsSpent);
            writer.WriteNumber("superguards", stats.Superguards);
            writer.WriteNumber("conditions_met", stats.ConditionsMet);
            writer.WriteEndObject();
        }
    }
}
