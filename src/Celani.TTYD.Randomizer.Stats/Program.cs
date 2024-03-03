using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using Celani.TTYD.Randomizer.Tracker;


var run = JsonSerializer.Deserialize<FullRun>(File.ReadAllText("/Users/mcelani/Downloads/pitrun-2024-02-26-07-54-12-4991.json"));

foreach (var floor in run.FloorSnapshots)
{
    var floorObj = JsonSerializer.Serialize(new {
        floor.Floor,
        Stats = new PlayerPouch(floor.FloorEndPouch),
        ModInfo = new InfinitePitStats(floor.FloorEndStats),
        Duration = TimeSpan.FromMilliseconds(floor.FloorDuration)
    }, new JsonSerializerOptions { WriteIndented = true });

    Console.WriteLine(floorObj);
}

public class FloorSnapshot
{
    [JsonPropertyName("floor")]
    public int Floor { get; init; }

    [JsonPropertyName("pouch")]
    public byte[] FloorEndPouch { get; init; }

    [JsonPropertyName("mod_data")]
    public byte[] FloorEndStats { get; init; }

    [JsonPropertyName("duration")]
    public long FloorDuration { get; init; }
}

public class FullRun
{
    [JsonPropertyName("floors")]
    public List<FloorSnapshot> FloorSnapshots { get; set; }
}