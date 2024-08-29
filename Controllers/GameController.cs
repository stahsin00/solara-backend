using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Net.WebSockets;
using System.Text;
using Solara.Data;
using Solara.Models;
using Solara.Services;
using Solara.Dtos;

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
        private readonly GameManager _gameManager;

        private readonly ILogger<GameController> _logger;

        public GameController(ApplicationContext context, ILogger<GameController> logger, UserSocketManager userSocketManager, RedisCacheService redis, GameManager gameManager)
        {
            _context = context;
            _logger = logger;
            _userSocketManager = userSocketManager;
            _redis = redis;
            _gameManager = gameManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGame([FromBody] GameDto dto)
        {
            try {
                var email = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest(new { message = "Invalid email claim." });
                }

                var user = await _context.Users.Include(u => u.Quests)
                                            .FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                Quest? quest = user.Quests.FirstOrDefault(q => q.Id == dto.Id);
                if (quest == null)
                {
                    return NotFound(new { message = "Quest not found." });
                }

                var game = new Game 
                {
                    User = user,
                    Quest = quest,
                    RemainingTime = TimeSpan.FromMinutes(dto.Minutes),
                    EnemyCurHealth = 100,
                    EnemyMaxHealth = 100,
                    DPS = 5,
                    CritRate = 0.5f,
                    CritDamage = 100,
                    Running = false
                };

                _context.Games.Add(game);
                await _context.SaveChangesAsync();

                return Ok(game);
            } catch (Exception e) {
                _logger.LogError(e, "Error in GameController - CreateGame:");
                return StatusCode(500, new { message = "Unable to create game." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            try {
                var email = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest(new { message = "Invalid email claim." });
                }

                var game = await _context.Games.Include(g => g.User).FirstOrDefaultAsync(g => g.Id == id);
                if (game == null || game.User.Email != email)
                {
                    return NotFound(new { message = "Game not found." });
                }

                // TODO

                return Ok(game);
            } catch (Exception e) {
                _logger.LogError(e, "Error in GameController - CreateGame:");
                return StatusCode(500, new { message = "Unable to create game." });
            }
        }

        // GET /api/game
        [HttpGet("{id}")]
        public async Task StartGame(int id)
        {
            try {
                var email = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    HttpContext.Response.StatusCode = 400;
                    return;
                }

                var game = await _context.Games.Include(g => g.User).FirstOrDefaultAsync(g => g.Id == id);
                if (game == null || game.User.Email != email)
                {
                    HttpContext.Response.StatusCode = 404;
                    return;
                }

                if (HttpContext.WebSockets.IsWebSocketRequest)
                {
                    if (_userSocketManager.HasActiveConnection(game.User.Id)) {  // TODO
                        HttpContext.Response.StatusCode = 400;
                    }

                    var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

                    await _gameManager.StartGame(game.Id);  // TODO

                    game.Running = true;
                    await _context.SaveChangesAsync();

                    await _redis.SetHashAsync(game.Id.ToString(), game);

                    await HandleWebSocketCommunication(game.User.Id, game.Id, webSocket);
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
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await _userSocketManager.CloseSocket(userId);
            await _redis.RemoveHashAsync<Game>(gameId.ToString());

            var game = await _context.Games.FindAsync(gameId);
            if (game != null)
            {
                game.Running = false;
                game.LastUpdate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }
            else
            {
                _logger.LogError($"Game with ID {gameId} not found in the database.");
            }

            _logger.LogInformation($"WebSocket connection closed for user {userId}");
        }
    }
}
