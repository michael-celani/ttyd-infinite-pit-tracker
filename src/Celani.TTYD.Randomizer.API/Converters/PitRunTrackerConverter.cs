using System.Text.Json.Serialization;
using System.Text.Json;
using System;

namespace Celani.TTYD.Randomizer.Tracker.Converters
{
    public class PitRunTrackerConverter : JsonConverter<PitRun>
    {
        public override PitRun Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, PitRun value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString("FileName", value.Data.FileName);
            WritePlayerPouch(writer, "PouchData", value.Data.Pouch);
            WriteInfinitePitStats(writer, "ModData", value.Data.ModInfo);
            WriteTimeSpan(writer, "PitRunElapsed", value.GetRunElapsed());
            WriteTimeSpan(writer, "FloorRunElapsed", value.GetFloorElapsed());
            writer.WriteEndObject();
        }

        private static void WritePlayerPouch(Utf8JsonWriter writer, string name, byte[] value)
        {
            var stats = new PlayerPouch(value);

            writer.WriteStartObject(name);

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

        private static void WriteInfinitePitStats(Utf8JsonWriter writer, string name, byte[] value)
        {
            var stats = new InfinitePitStats(value);

            writer.WriteStartObject(name);
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

        public void WriteTimeSpan(Utf8JsonWriter writer, string name, TimeSpan value)
        {
            var totalHours = value.TotalHours;

            if (totalHours <= 0)
            {
                writer.WriteString(name, "00:00:00.00");
                return;
            }

            if (totalHours >= 100)
            {
                writer.WriteString(name, "99:59:59.99");
                return;
            }

            Span<char> arr = stackalloc char[11];
            var hoursInt = (int)totalHours;
            hoursInt.TryFormat(arr[0..2], out var charsWritten, "00");
            value.TryFormat(arr[2..], out charsWritten, @"\:mm\:ss\.ff");
            writer.WriteString(name, arr);
        }
    }
}
