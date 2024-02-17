using Celani.TTYD.Randomizer.API;
using Celani.TTYD.Randomizer.Tracker;
using Celani.TTYD.Randomizer.Tracker.Dolphin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
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
        public async Task GetPouch()
        {
            /*
            if (!ValidateOrigin(HttpContext.Request.Headers.Origin))
            {
                HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
            */
            
            if (!HttpContext.WebSockets.IsWebSocketRequest)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }

            if (!TryConnectDolphin(out ThousandYearDoorTracker tracker))
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }

            // Accept the websocket.
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

            var run = new PitRun();
            Task sendTask = SendPouchDataAsync(tracker, webSocket, run);
            Task heartbeatTask = RecieveHeartbeatAsync(webSocket);

            try
            {
                await Task.WhenAny(sendTask, heartbeatTask).ConfigureAwait(false);
            }
            catch (Exception)
            {
                // Do nothing.
            }
        }

        private static void GetData(ThousandYearDoorTracker tracker, PitRun run, MemoryStream stream)
        {
            // Get values;
            tracker.Update();
            ref var pouchData = ref tracker.GetPouchData();
            ref var modData   = ref tracker.GetModData();

            DateTime runStart = GamecubeGame.DateTimeFromGCNTick(modData.PitStartTime);
            DateTime now = GamecubeGame.DateTimeFromGCNTick(tracker.Tick);
            TimeSpan runElapsed = now - runStart;

            if (!run.IsStarted && modData.PitStartTime != 0)
            {
                // The pit is not started, but just began.
                run.IsStarted = true;
                run.CurrentFloor = modData.Floor;
                run.CurrentFloorStart = GamecubeGame.DateTimeFromGCNTick(tracker.Tick);
            }
            else if (run.IsStarted && modData.PitStartTime == 0)
            {
                // The pit was started, but just ended.
                run.IsStarted = false;
                run.CurrentFloor = 0;
                run.CurrentFloorStart = null;
            }

            TimeSpan floorElapsed = run.CurrentFloorStart.HasValue ? now - run.CurrentFloorStart.Value : TimeSpan.Zero;

            if (modData.Floor != run.CurrentFloor)
            {
                // The floor has changed.
                run.CurrentFloor = modData.Floor;
                run.CurrentFloorStart = now;
            }

            var sentData = new SentData
            {
                FileName = tracker.FileName,
                PouchData = pouchData,
                ModData = modData,
                PitRunStart = runStart.ToString("R"),
                PitRunElapsed = runElapsed.ToString(@"hh\:mm\:ss\.ff"),
                FloorRunElapsed = floorElapsed.ToString(@"hh\:mm\:ss\.ff"),
            };

            // Send the data.
            JsonSerializer.Serialize(stream, sentData);
        }

        private static async Task SendPouchDataAsync(ThousandYearDoorTracker tracker, WebSocket webSocket, PitRun run)
        {
            MemoryStream stream = new();

            while (webSocket.State == WebSocketState.Open)
            {
                // Wait 1/60th of a second
                Task timeTask = Task.Delay(WaitTime);

                GetData(tracker, run, stream);

                if (stream.Position != 0)
                {
                    await webSocket.SendAsync(stream.GetBuffer()[0..(Index) stream.Position], WebSocketMessageType.Text, true, CancellationToken.None).ConfigureAwait(false);
                }
                
                stream.Seek(0, SeekOrigin.Begin);
                
                await timeTask.ConfigureAwait(false);
            }
        }

        private static async Task RecieveHeartbeatAsync(WebSocket webSocket)
        {
            var reciveBuffer = new byte[32000];

            while (webSocket.State == WebSocketState.Open)
            {
                CancellationTokenSource source = new();
                source.CancelAfter(HeartbeatPeriod);

                // Receive a heartbeat.
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(reciveBuffer), source.Token).ConfigureAwait(false);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None).ConfigureAwait(false);
                }
            }
        }

        private static bool ValidateOrigin(StringValues origin)
        {
            if (origin.Count == 0)
            {
                return false;
            }

            var originStr = origin.First();
            var originUri = new Uri(originStr);
            return originUri.Host == "localhost" || originUri.Host == "gamesfreaksa.info";
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
