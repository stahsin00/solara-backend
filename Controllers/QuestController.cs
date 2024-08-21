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
    [Authorize]
    public class QuestController : ControllerBase
    {
        private readonly ApplicationContext _context;

        private readonly ILogger<QuestController> _logger;

        public QuestController(ApplicationContext context, ILogger<QuestController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET /api/quest
        [HttpGet]
        public async Task<IActionResult> GetAllQuests()
        {
            try {
                var email = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest("Invalid email claim.");
                }

                var user = await _context.Users.Include(u => u.Quests)  // TODO: consider pagination
                                            .FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                return Ok(user.Quests);
            } catch (Exception e) {
                _logger.LogError(e, "Error in QuestController - GetAllQuests:");
                return StatusCode(500, new { message = "Unable to get quests." });
            }
        }
    }
}
