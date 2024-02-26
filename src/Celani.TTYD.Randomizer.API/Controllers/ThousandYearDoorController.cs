using Celani.TTYD.Randomizer.API.Filters;
using Celani.TTYD.Randomizer.API.Models;
using Celani.TTYD.Randomizer.Tracker;
using Celani.TTYD.Randomizer.Tracker.Dolphin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Celani.TTYD.Randomizer.UI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ThousandYearDoorController() : ControllerBase
    {
        private static readonly TimeSpan WaitTime = TimeSpan.FromMilliseconds(100.0 / 6.0);

        private static readonly TimeSpan HeartbeatPeriod = TimeSpan.FromSeconds(5);

        [Route("/pouch")]
        [ValidateOriginFilter]
        [WebsocketsOnlyFilter]
        public async Task GetPouch()
        {
            if (!TryConnectDolphin(out ThousandYearDoorTracker tracker))
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }

            // Accept the websocket.
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

            var flags = new PitRunFlags();
            var run = new PitRun();
            
            run.OnPitStart += (sender, args) =>
            {
                tracker.UpdateFilename();
                flags.ShouldWrite = false;
            };

            run.OnPitReset += (sender, args) =>
            {
                tracker.UpdateFilename();
                flags.ShouldWrite = false;
            };

            run.OnPitFinish += (sender, args) =>
            {
                flags.ShouldWrite = true;
            };

            Task sendTask = SendPouchDataAsync(tracker, webSocket, run, flags);
            Task heartbeatTask = RecieveHeartbeatAsync(webSocket);
            await Task.WhenAny(sendTask, heartbeatTask);
        }


        private static void GetData(ThousandYearDoorTracker tracker, PitRun run, MemoryStream stream)
        {
            // Get values;
            tracker.Update();

            DateTime runStart = GamecubeGame.DateTimeFromGCNTick(tracker.ModInfo.PitStartTime);
            DateTime now = GamecubeGame.DateTimeFromGCNTick(tracker.ModInfo.PitFinished ? tracker.ModInfo.PitEndTime : tracker.Tick);
            TimeSpan runElapsed = now - runStart;

            run.Update(tracker.Pouch, tracker.ModInfo, now);

            var sentData = new SentData
            {
                FileName = tracker.FileName,
                PouchData = tracker.Pouch,
                ModData = tracker.ModInfo,
                PitRunElapsed = runElapsed,
                FloorRunElapsed = run.GetFloorElapsed(now),
            };

            // Send the data.
            JsonSerializer.Serialize(stream, sentData);
        }

        private static async Task SendPouchDataAsync(ThousandYearDoorTracker tracker, WebSocket webSocket, PitRun run, PitRunFlags flags)
        {
            MemoryStream stream = new();

            while (webSocket.State == WebSocketState.Open)
            {
                // Wait 1/60th of a second
                Task timeTask = Task.Delay(WaitTime);

                GetData(tracker, run, stream);

                if (stream.Position != 0)
                {
                    var memory = stream.GetBuffer().AsMemory()[0..(Index) stream.Position];
                    await webSocket.SendAsync(memory, WebSocketMessageType.Text, true, CancellationToken.None);
                }
                
                stream.Seek(0, SeekOrigin.Begin);

                if (flags.ShouldWrite)
                {
                    flags.ShouldWrite = false;
                    await WriteRunDataAsync(run);
                }
                
                await timeTask;
            }
        }

        private static async Task WriteRunDataAsync(PitRun run)
        {
            var fileName = @$"pitrun-{DateTime.Now:yyyy-MM-dd-hh-mm-ss-ffff}.json";
            using FileStream stream = System.IO.File.Create(fileName);
            await JsonSerializer.SerializeAsync(stream, run);
        }

        private static async Task RecieveHeartbeatAsync(WebSocket webSocket)
        {
            var reciveBuffer = new byte[32000];

            while (webSocket.State == WebSocketState.Open)
            {
                using CancellationTokenSource source = new();
                source.CancelAfter(HeartbeatPeriod);

                // Receive a heartbeat.
                var result = await webSocket.ReceiveAsync(reciveBuffer.AsMemory(), source.Token);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                }
            }
        }

        private static bool TryConnectDolphin(out ThousandYearDoorTracker tracker)
        {
            Process dolphinProcess = Process.GetProcessesByName("dolphin").FirstOrDefault();

            if (dolphinProcess is null)
            {
                tracker = null;
                return false;
            }

            // Todo: validate game is TTYD
            var game = GamecubeGame.Create(dolphinProcess);
            tracker = new(game);

            return true;
        }
    }
}
