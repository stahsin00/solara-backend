using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Solara.Data;
using Solara.Models;
using Solara.Dtos;

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
                    return BadRequest(new { message = "Invalid email claim." });
                }

                var user = await _context.Users.Include(u => u.Quests)  // TODO: consider pagination
                                            .FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                var quests = user.Quests.Where(q => !q.Complete).ToList();  // TODO: filter when retrieving from database

                return Ok(quests);
            } catch (Exception e) {
                _logger.LogError(e, "Error in QuestController - GetAllQuests:");
                return StatusCode(500, new { message = "Unable to get quests." });
            }
        }

        // GET /api/quest/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetQuest(int id)
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

                var quest = user.Quests.FirstOrDefault(q => q.Id == id);
                if (quest == null)
                {
                    return NotFound(new { message = "Quest not found." });
                }

                return Ok(quest);
            } catch (Exception e) {
                _logger.LogError(e, "Error in QuestController - GetQuest:");
                return StatusCode(500, new { message = "Unable to get quest." });
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
                    return BadRequest(new { message = "Invalid difficulty. Difficulty must be one of the following: Easy, Medium, Hard, Unspecified." });
                }

                if (!Enum.TryParse<Repetition>(dto.Repetition, true, out var repetition) && !string.IsNullOrEmpty(dto.Repetition))
                {
                    return BadRequest(new { message = $"Invalid repetition type: {dto.Repetition}" });
                }
                
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

        // PATCH /api/quest/{id}
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> EditQuest(int id, [FromBody] QuestDto dto)
        {
            try {
                // TODO: validation
                if (!Enum.TryParse<Difficulty>(dto.Difficulty, true, out var difficulty))
                {
                    return BadRequest(new { message = "Invalid difficulty. Difficulty must be one of the following: Easy, Medium, Hard, Unspecified." });
                }

                if (!Enum.TryParse<Repetition>(dto.Repetition, true, out var repetition) && !string.IsNullOrEmpty(dto.Repetition))
                {
                    return BadRequest(new { message = $"Invalid repetition type: {dto.Repetition}" });
                }
                
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

                var quest = user.Quests.FirstOrDefault(q => q.Id == id);
                if (quest == null)
                {
                    return NotFound(new { message = "Quest not found." });
                }

                quest.Name = dto.Name;
                quest.Description = dto.Description;
                quest.Deadline = dto.Deadline;
                quest.Difficulty = difficulty;
                quest.Important = dto.Important;
                quest.UpdatedAt = DateTime.UtcNow;

                if (!string.IsNullOrEmpty(dto.Repetition))
                {
                    // TODO
                }

                await _context.SaveChangesAsync();

                return Ok(quest);
            } catch (Exception e) {
                _logger.LogError(e, "Error in QuestController - EditQuest:");
                return StatusCode(500, new { message = "Unable to edit quest." });
            }
        }

        // DELETE /api/quest/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteQuest(int id)
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

                var quest = user.Quests.FirstOrDefault(q => q.Id == id);
                if (quest == null)
                {
                    return NotFound(new { message = "Quest not found." });
                }

                _context.Quests.Remove(quest);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Quest deleted." });
            } catch (Exception e) {
                _logger.LogError(e, "Error in QuestController - DeleteQuest:");
                return StatusCode(500, new { message = "Unable to delete quest." });
            }
        }

        // PATCH /api/quest/complete/{id}
        [HttpPatch("complete/{id:int}")]
        public async Task<IActionResult> CompleteQuest(int id)
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

                var quest = user.Quests.FirstOrDefault(q => q.Id == id);
                if (quest == null)
                {
                    return NotFound(new { message = "Quest not found." });
                }

                if (quest.Complete) {
                    return BadRequest(new { message = "Quest is already completed." });
                }

                // TODO
                var balance = 0;
                var exp = 0;

                switch (quest.Difficulty) {
                    case Difficulty.Medium:
                        balance = 500;
                        exp = 5;
                        break;
                    case Difficulty.Hard:
                        balance = 1000;
                        exp = 10;
                        break;
                    default:
                        balance = 100;
                        exp = 1;
                        break;
                }

                quest.Complete = true;
                user.Balance += balance;
                user.Exp += exp;
                quest.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Quest completed." });
            } catch (Exception e) {
                _logger.LogError(e, "Error in QuestController - CompleteQuest:");
                return StatusCode(500, new { message = "Unable to complete quest." });
            }
        }
    }
}
