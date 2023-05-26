using Celani.TTYD.Randomizer.API;
using Celani.TTYD.Randomizer.Tracker;
using Celani.TTYD.Randomizer.Tracker.Dolphin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Celani.TTYD.Randomizer.UI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ThousandYearDoorController : ControllerBase
    {
        /// <summary>
        /// A Logger.
        /// </summary>
        private ILogger<ThousandYearDoorController> Logger { get; set; }

        public ThousandYearDoorController(ILogger<ThousandYearDoorController> logger)
        {
            Logger = logger;
        }

        [Route("/pouch")]
        public async Task GetPouch()
        {
            if (!ValidateOrigin(HttpContext.Request.Headers.Origin))
            {
                HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

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

            Task sendTask = SendPouchDataAsync(tracker, webSocket);
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

        private static async Task SendPouchDataAsync(ThousandYearDoorTracker tracker, WebSocket webSocket)
        {
            var waitTime = TimeSpan.FromMilliseconds(100.0 / 6.0);

            var serializerOptions = new JsonSerializerOptions { 
                IncludeFields = true,
            };

            // Get initial values.
            tracker.Update();
            var isStarted = tracker.ModData.pit_start_time != 0;
            var prevFloor = tracker.ModData.floor;
            var prevFloorStart = GamecubeGame.DateTimeFromGCNTick(tracker.Tick);

            while (webSocket.State == WebSocketState.Open)
            {
                // Wait 1/60th of a second
                Task timeTask = Task.Delay(waitTime);

                tracker.Update();

                DateTime runStart = GamecubeGame.DateTimeFromGCNTick(tracker.ModData.pit_start_time);
                DateTime now = GamecubeGame.DateTimeFromGCNTick(tracker.Tick);
                TimeSpan runElapsed = now - runStart;

                if (!isStarted && tracker.ModData.pit_start_time != 0)
                {
                    prevFloor = tracker.ModData.floor;
                    prevFloorStart = now;
                    isStarted = true;
                    Console.WriteLine($"Run Started: Seed {tracker.FileName}");
                }

                if (tracker.ModData.floor != prevFloor)
                {
                    TimeSpan floorElapsed = now - prevFloorStart;
                    var floorElapsedstr = floorElapsed.ToString(@"hh\:mm\:ss\.ff");

                    Console.WriteLine($"Floor {prevFloor + 1}: {floorElapsedstr}");

                    prevFloorStart = now;
                    prevFloor = tracker.ModData.floor;
                }

                var sentData = new SentData
                {
                    FileName = tracker.FileName,
                    PouchData = tracker.PouchData,
                    ModData = tracker.ModData,
                    PitRunStart = runStart.ToString("R"),
                    PitRunElapsed = runElapsed.ToString(@"hh\:mm\:ss\.ff")
                };

                // Send the data.
                string data = JsonSerializer.Serialize(sentData, serializerOptions);
                byte[] send = Encoding.UTF8.GetBytes(data);
                await webSocket.SendAsync(send, WebSocketMessageType.Text, true, CancellationToken.None).ConfigureAwait(false);

                await timeTask.ConfigureAwait(false);
            }
        }

        private static async Task RecieveHeartbeatAsync(WebSocket webSocket)
        {
            var reciveBuffer = new byte[32000];
            var timeoutTime = TimeSpan.FromSeconds(5);

            while (webSocket.State == WebSocketState.Open)
            {
                CancellationTokenSource source = new();
                source.CancelAfter(timeoutTime);

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
            if (!origin.Any())
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
            GamecubeGame game = GamecubeGame.Create(dolphinProcess);
            tracker = new(game);

            return true;
        }
    }
}
