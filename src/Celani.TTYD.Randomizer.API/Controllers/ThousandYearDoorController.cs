using Celani.TTYD.Randomizer.API;
using Celani.TTYD.Randomizer.API.Filters;
using Celani.TTYD.Randomizer.Tracker.Dolphin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Celani.TTYD.Randomizer.UI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ThousandYearDoorController : ControllerBase
    {
        [Route("/pouch")]
        [ValidateOriginFilter]
        [WebsocketsOnlyFilter]
        public async Task GetPouch()
        {
            Process[] dolphinProcess = Process.GetProcessesByName("dolphin");

            if (dolphinProcess.Length == 0)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                return;
            }

            if (!GamecubeGame.TryAttach(dolphinProcess[0], out var game))
            {
                HttpContext.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                return;
            }

            // Accept the websocket.
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

            await InfinitePitTracker.TrackAsync(game, webSocket);
        }
    }
}
