using System;
using System.Diagnostics;
using System.IO;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Celani.TTYD.Randomizer.Tracker;
using Celani.TTYD.Randomizer.Tracker.Dolphin;

namespace Celani.TTYD.Randomizer.API.Models
{
    public class InfinitePitTracker
    {
        private static readonly TimeSpan WaitTime = TimeSpan.FromMilliseconds(100.0 / 6.0);

        private static readonly TimeSpan HeartbeatPeriod = TimeSpan.FromSeconds(5);

        public PitRun Run { get; set;} = new(ConnectDolphin());

        public bool ShouldWrite { get; set; } = false;

        public InfinitePitTracker()
        {
            Run.OnPitStart += (sender, args) => ShouldWrite = false;
            Run.OnPitReset += (sender, args) => ShouldWrite = false;
            Run.OnPitFinish += (sender, args) => ShouldWrite = true;
        }

        public async Task TrackAsync(WebSocket socket) => await SendPouchDataAsync(socket);

        private static ThousandYearDoorDataReader ConnectDolphin()
        {
            Process[] dolphinProcess = Process.GetProcessesByName("dolphin");

            if (dolphinProcess.Length == 0)
            {
                throw new InvalidOperationException("Dolphin is not running.");
            }

            var game = GamecubeGame.Create(dolphinProcess[0]);
            return new(game);
        }

        private async Task SendPouchDataAsync(WebSocket webSocket)
        {
            MemoryStream stream = new();

            while (webSocket.State == WebSocketState.Open)
            {
                // Wait 1/60th of a second
                Task timeTask = Task.Delay(WaitTime);

                Run.Update();

                var sentData = new SentData(Run);

                JsonSerializer.Serialize(stream, sentData);

                if (stream.Position != 0)
                {
                    var memory = stream.GetBuffer().AsMemory()[0..(Index) stream.Position];
                    await webSocket.SendAsync(memory, WebSocketMessageType.Text, true, CancellationToken.None);
                }
                
                stream.Seek(0, SeekOrigin.Begin);

                if (ShouldWrite)
                {
                    ShouldWrite = false;
                    await WriteRunDataAsync();
                }
                
                await timeTask;
            }
        }

        private async Task WriteRunDataAsync()
        {
            var fileName = @$"pitrun-{DateTime.Now:yyyy-MM-dd-hh-mm-ss-ffff}.json";
            using FileStream stream = File.Create(fileName);
            await JsonSerializer.SerializeAsync(stream, Run);
        }
    }
}
