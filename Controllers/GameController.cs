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
        private readonly GameManager _gameManager;

        private readonly ILogger<GameController> _logger;

        public GameController(ApplicationContext context, ILogger<GameController> logger, UserSocketManager userSocketManager, GameManager gameManager)
        {
            _context = context;
            _logger = logger;
            _userSocketManager = userSocketManager;
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

                var dbGame = await _context.Games.FirstOrDefaultAsync(g => g.UserId == user.Id);  // TODO

                if (dbGame != null) {
                    return BadRequest(new { message = "User already has an existing game." });
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

        [HttpDelete]
        public async Task<IActionResult> DeleteGame()
        {
            try {
                var email = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest(new { message = "Invalid email claim." });
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                var game = await _context.Games.FirstOrDefaultAsync(g => g.UserId == user.Id);  // TODO

                if (game == null) {
                    return BadRequest(new { message = "This user does not have a game." });
                }

                if (game.Running) {
                    return BadRequest(new { message = "Cannot delete a running game." });
                }

                user.Balance += game.RewardBalance;
                user.Exp += game.RewardExp;
                _context.Games.Remove(game);
                await _context.SaveChangesAsync();

                return Ok();
            } catch (Exception e) {
                _logger.LogError(e, "Error in GameController - DeleteGame:");
                return StatusCode(500, new { message = "Unable to delete game." });
            }
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

                var game = await _context.Games.FirstOrDefaultAsync(g => g.UserId == user.Id);  // TODO

                if (game == null)
                {
                    HttpContext.Response.StatusCode = 404;
                    return;
                }

                if (HttpContext.WebSockets.IsWebSocketRequest)
                {
                    if (_userSocketManager.HasActiveConnection(user.Id)) {
                        HttpContext.Response.StatusCode = 400;
                    }

                    var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

                    await _gameManager.StartGame(game);

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
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await _gameManager.StopGame(gameId);
            await _userSocketManager.CloseSocket(userId);

            _logger.LogInformation($"WebSocket connection closed for user {userId}");
        }
    }
}
