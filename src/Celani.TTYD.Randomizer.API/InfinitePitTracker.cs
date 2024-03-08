using System;
using System.IO;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Celani.TTYD.Randomizer.Tracker;
using Celani.TTYD.Randomizer.Tracker.Converters;
using Celani.TTYD.Randomizer.Tracker.Dolphin;

namespace Celani.TTYD.Randomizer.API
{
    public static class InfinitePitTracker
    {
        private static readonly TimeSpan WaitTime = TimeSpan.FromMilliseconds(100.0 / 6.0);

        private static readonly JsonSerializerOptions FileOptions = GetFileOptions();

        private static readonly JsonSerializerOptions TrackerOptions = GetTrackerOptions();

        public static async Task TrackAsync(GamecubeGame game, WebSocket webSocket)
        {
            var sendTask = SendAsync(game, webSocket);
            var recvTask = ReceiveAsync(webSocket);
            await Task.WhenAny(sendTask, recvTask);
        }

        public static async Task SendAsync(GamecubeGame game, WebSocket webSocket)
        {
            var data = new ThousandYearDoorDataReader(game);
            var run = new PitRun(data);

            var shouldWrite = false;
            run.OnPitStart += (sender, args) => shouldWrite = false;
            run.OnPitReset += (sender, args) => shouldWrite = false;
            run.OnPitFinish += (sender, args) => shouldWrite = true;

            MemoryStream stream = new();

            using PeriodicTimer timer = new(WaitTime);

            while (webSocket.State == WebSocketState.Open)
            {
                if (!game.Running || !run.Update())
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "The Thousand Year Door has closed.", CancellationToken.None);
                    return;
                }

                JsonSerializer.Serialize(stream, run, TrackerOptions);

                if (stream.Position != 0)
                {
                    var memory = stream.GetBuffer().AsMemory()[0..(Index)stream.Position];
                    await webSocket.SendAsync(memory, WebSocketMessageType.Text, true, CancellationToken.None);
                }

                stream.Seek(0, SeekOrigin.Begin);

                if (shouldWrite)
                {
                    shouldWrite = false;
                    await WriteRunDataAsync(run);
                }

                await timer.WaitForNextTickAsync(CancellationToken.None);
            }
        }

        public static async Task ReceiveAsync(WebSocket webSocket)
        {
            Memory<byte> buffer = new byte[1024 * 4];

            while (webSocket.State == WebSocketState.Open)
            {
                await webSocket.ReceiveAsync(buffer, CancellationToken.None);
            }
        }

        private static async Task WriteRunDataAsync(PitRun run)
        {
            var fileName = @$"pitrun-{DateTime.Now:yyyy-MM-dd-hh-mm-ss-ffff}.json";
            using FileStream stream = File.Create(fileName);
            await JsonSerializer.SerializeAsync(stream, run, FileOptions);
        }

        private static JsonSerializerOptions GetFileOptions()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new PitRunFileConverter());

            return options;
        }

        private static JsonSerializerOptions GetTrackerOptions()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new PitRunTrackerConverter());

            return options;
        }
    }
}
