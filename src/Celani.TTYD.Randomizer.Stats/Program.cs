using System.Text.Json;
using System.Text.Json.Serialization;
using Celani.TTYD.Randomizer.Tracker;
using Celani.TTYD.Randomizer.Tracker.Converters;

var text = File.ReadAllText("/Users/mcelani/Downloads/pitrun-2024-02-26-07-54-12-4991.json");
var run = JsonSerializer.Deserialize<PitLog>(text);
var options = new JsonSerializerOptions { WriteIndented = true };

using var file = File.OpenWrite("/Users/mcelani/Desktop/out.json");
List<object> list = new();

foreach (var floor in run.FloorSnapshots)
{
    list.Add(new FloorNew
    {
        Floor = floor.Floor,
        FloorEndPouch = floor.FloorEndPouch,
        FloorEndStats = floor.FloorEndStats,
        FloorDuration = floor.FloorDuration
    });
}

JsonSerializer.Serialize(file, list, options);

public class FloorNew
{
    [JsonPropertyName("floor")]
    public int Floor { get; init; }

    [JsonPropertyName("pouch")]
    [JsonConverter(typeof(PlayerStatsConverter))]
    public byte[] FloorEndPouch { get; init; }

    [JsonPropertyName("mod_data")]
    [JsonConverter(typeof(ModDataConverter))]
    public byte[] FloorEndStats { get; init; }

    [JsonPropertyName("duration")]
    public TimeSpan FloorDuration { get; init; }
}