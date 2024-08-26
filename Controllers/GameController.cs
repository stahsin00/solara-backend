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
        private readonly RedisCacheService _redis;

        private readonly ILogger<GameController> _logger;

        public GameController(ApplicationContext context, ILogger<GameController> logger, UserSocketManager userSocketManager, RedisCacheService redis)
        {
            _context = context;
            _logger = logger;
            _userSocketManager = userSocketManager;
            _redis = redis;
        }

        // GET /api/game
        [HttpGet("{id}")]
        public async Task StartGame(int id)
        {
            _logger.LogInformation("1");
            try {
                var email = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
                _logger.LogInformation($"{email}");
                if (string.IsNullOrEmpty(email))
                {
                    HttpContext.Response.StatusCode = 400;
                    return;
                }

                var user = await _context.Users.Include(u => u.Quests)
                                            .FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    HttpContext.Response.StatusCode = 404;
                    return;
                }
                _logger.LogInformation($"{user}");

                var quest = user.Quests.FirstOrDefault(q => q.Id == id);
                if (quest == null)
                {
                    HttpContext.Response.StatusCode = 404;
                    return;
                }
                _logger.LogInformation($"{quest}");

                if (HttpContext.WebSockets.IsWebSocketRequest)
                {
                    // TODO: consider if the user already has an active connection

                    var game = new Game 
                    {
                        User = user,
                        Quest = quest,
                        RemainingTime = TimeSpan.FromMinutes(2),
                        EnemyCurHealth = 100,
                        EnemyMaxHealth = 100,
                        DPS = 5,
                        CritRate = 0.5f,
                        CritDamage = 100
                    };
                    _logger.LogInformation($"{game}");

                    _context.Games.Add(game);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("context");
                    var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                    _logger.LogInformation("websocket");
                    await _redis.SetHashAsync(game.Id.ToString(), game);
                    _logger.LogInformation("redis");

                    await HandleWebSocketCommunication(user.Id, game.Id, webSocket);
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

        private async Task HandleWebSocketCommunication(int userId, int gameId, WebSocket webSocket) {
            await _userSocketManager.AddSocket(userId, webSocket);

            var buffer = new byte[1024 * 4];  // TODO: buffer size can be configured centrally
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!result.CloseStatus.HasValue)
            {
                // TODO: pause/play?
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            // TODO: database updates
            await _userSocketManager.CloseSocket(userId);
            await _redis.RemoveHashAsync<Game>(gameId.ToString());
            _logger.LogInformation($"WebSocket connection closed for user {userId}");

        }
    }
}
