using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Net.WebSockets;
using System.Text;
using Solara.Data;
using Solara.Models;
using Solara.Services;

namespace Solara.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GameController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly UserSocketManager _userSocketManager;

        private readonly ILogger<GameController> _logger;

        public GameController(ApplicationContext context, ILogger<GameController> logger, UserSocketManager userSocketManager)
        {
            _context = context;
            _logger = logger;
            _userSocketManager = userSocketManager;
        }

        // GET /api/game
        [HttpGet]
        public async Task StartGame()
        {
            try {
                var email = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    HttpContext.Response.StatusCode = 400;
                    return;
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    HttpContext.Response.StatusCode = 404;
                    return;
                }

                if (HttpContext.WebSockets.IsWebSocketRequest)
                {
                    // TODO: consider if the user already has an active connection
                    var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                    var userId = user.Id;

                    await HandleWebSocketCommunication(userId, webSocket);
                }
                else
                {
                    HttpContext.Response.StatusCode = 400;
                }

            } catch (Exception e) {
                _logger.LogError(e, "Error in GameController - StartGame:");
                HttpContext.Response.StatusCode = 500;
            }
        }

        private async Task HandleWebSocketCommunication(int userId, WebSocket webSocket) {
            await _userSocketManager.AddSocket(userId, webSocket);

            var buffer = new byte[1024 * 4];  // TODO: buffer size can be configured centrally
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!result.CloseStatus.HasValue)
            {
                // TODO: pause/play?
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            // TODO: database updates
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            await _userSocketManager.RemoveSocket(userId);
            _logger.LogInformation($"WebSocket connection closed for user {userId}");

        }
    }
}
