using Celani.TTYD.Randomizer.API.Filters;
using Celani.TTYD.Randomizer.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Celani.TTYD.Randomizer.UI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ThousandYearDoorController() : ControllerBase
    {
        [Route("/pouch")]
        [ValidateOriginFilter]
        [WebsocketsOnlyFilter]
        public async Task GetPouch()
        {
            InfinitePitTracker tracker;

            try
            {
                tracker = new InfinitePitTracker();
            }
            catch (InvalidOperationException)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                return;
            }

            // Accept the websocket.
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

            await tracker.TrackAsync(webSocket);
        }
    }
}
