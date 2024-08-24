using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Solara.Data;
using Solara.Models;

namespace Solara.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly ApplicationContext _context;

        private readonly ILogger<GameController> _logger;

        public GameController(ApplicationContext context, ILogger<GameController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET /api/game
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GameInfo()
        {
            try {
                var email = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest("Invalid email claim.");
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                // TODO

                return Ok(new { message = "TODO" });
            } catch (Exception e) {
                _logger.LogError(e, "Error in GameController - GameInfo:");
                return StatusCode(500, new { message = "Unable to get game." });
            }
        }

        // POST /api/game
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> StartGame()
        {
            try {
                var email = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest("Invalid email claim.");
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                // TODO

                return Ok(new { message = "TODO" });
            } catch (Exception e) {
                _logger.LogError(e, "Error in GameController - StartGame:");
                return StatusCode(500, new { message = "Unable to start game." });
            }
        }

        // PATCH /api/game
        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> PausePlayGame()
        {
            try {
                var email = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest("Invalid email claim.");
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                // TODO

                return Ok(new { message = "TODO" });
            } catch (Exception e) {
                _logger.LogError(e, "Error in GameController - PausePlayGame:");
                return StatusCode(500, new { message = "Unable to pause/play game." });
            }
        }

        // DELETE /api/game
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> StopGame()
        {
            try {
                var email = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest("Invalid email claim.");
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                // TODO

                return Ok(new { message = "TODO" });
            } catch (Exception e) {
                _logger.LogError(e, "Error in GameController - StopGame:");
                return StatusCode(500, new { message = "Unable to stop game." });
            }
        }
    }
}
