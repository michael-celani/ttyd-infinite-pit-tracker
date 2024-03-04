using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using Celani.TTYD.Randomizer.Tracker;


var run = JsonSerializer.Deserialize<FullRun>(File.ReadAllText("C:\\Users\\Michael Celani\\Desktop\\pitrun-2024-03-02-10-40-47-8239.json"));
var options = new JsonSerializerOptions { WriteIndented = true };

using var file = File.OpenWrite("C:\\Users\\Michael Celani\\Desktop\\out.json");
List<object> list = new();

foreach (var floor in run.FloorSnapshots)
{
    list.Add(new
    {
        floor = floor.Floor,
        stats = new PlayerPouch(floor.FloorEndPouch),
        mod_info = new InfinitePitStats(floor.FloorEndStats),
        duration = TimeSpan.FromMilliseconds(floor.FloorDuration)
    });
}

JsonSerializer.Serialize(file, list, options);

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