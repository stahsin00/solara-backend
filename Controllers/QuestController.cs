using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Solara.Data;
using Solara.Models;
using Solara.Dtos;
using System.Text.Json;
using System.IO;

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

        // POST /api/quest
        [HttpPost]
        public async Task<IActionResult> CreateQuest([FromBody] QuestDto dto)
        {
            try {
                // TODO: validation
                if (!Enum.TryParse<Difficulty>(dto.Difficulty, true, out var difficulty))
                {
                    return BadRequest("Invalid difficulty. Difficulty must be one of the following: Easy, Medium, Hard, Unspecified.");
                }

                if (!Enum.TryParse<Repetition>(dto.Repetition, true, out var repetition) && !string.IsNullOrEmpty(dto.Repetition))
                {
                    return BadRequest($"Invalid repetition type: {dto.Repetition}");
                }
                
                var email = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest("Invalid email claim.");
                }

                var user = await _context.Users.Include(u => u.Quests)
                                            .FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                Quest quest;

                if (!string.IsNullOrEmpty(dto.Repetition))
                {
                    quest = new RecurrentQuest
                    {
                        Name = dto.Name,
                        Description = dto.Description,
                        UserId = user.Id,
                        Deadline = dto.Deadline,
                        Difficulty = difficulty,
                        Important = dto.Important,
                        EndDate = dto.EndDate,
                        Repetition = repetition
                    };
                }
                else
                {
                    quest = new Quest
                    {
                        Name = dto.Name,
                        Description = dto.Description,
                        UserId = user.Id,
                        Deadline = dto.Deadline,
                        Difficulty = difficulty,
                        Important = dto.Important
                    };
                }

                user.Quests.Add(quest);
                await _context.SaveChangesAsync();

                return Ok(quest);
            } catch (Exception e) {
                _logger.LogError(e, "Error in QuestController - CreateQuest:");
                return StatusCode(500, new { message = "Unable to create quest." });
            }
        }
    }
}
