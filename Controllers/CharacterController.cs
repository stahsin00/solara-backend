using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Solara.Data;
using Solara.Models;

namespace Solara.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ApplicationContext _context;

        private readonly ILogger<CharacterController> _logger;

        public CharacterController(ApplicationContext context, ILogger<CharacterController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET /api/character
        [HttpGet]
        public async Task<IActionResult> GetAllCharacters()
        {
            var characters = await _context.Characters.ToListAsync();
            return Ok(characters);
        }

        // GET /api/character/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCharacterById(int id)
        {
            var character = await _context.Characters.FindAsync(id);
            return character != null ? Ok(character) : NotFound();
        }

        // GET /api/character/user
        [HttpGet("user")]
        [Authorize]
        public async Task<IActionResult> GetUserCharacters()
        {
            try {
                var email = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest("Invalid email claim.");
                }

                // TODO: consider pagination
                var user = await _context.Users.Include(u => u.Characters)
                                            .ThenInclude(ci => ci.Character)
                                            .FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                return Ok(user.Characters);
            } catch (Exception e) {
                _logger.LogError(e, "Error in CharacterController - GetUserCharacters:");
                return StatusCode(500, new { message = "Unable to get characters." });
            }
        }

        // GET /api/character/user/{id}
        [HttpGet("user/{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetUserCharacterById(int id)
        {
            try {
                var email = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest("Invalid email claim.");
                }

                var user = await _context.Users.Include(u => u.Characters)
                                            .ThenInclude(ci => ci.Character)  // TODO: consider not eager loading
                                            .FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                var characterInstance = user.Characters.FirstOrDefault(ci => ci.Character.Id == id);
                if (characterInstance == null)
                {
                    return NotFound(new { message = "Character not found." });
                }

                return Ok(characterInstance);
            } catch (Exception e) {
                _logger.LogError(e, "Error in CharacterController - GetUserCharacterById:");
                return StatusCode(500, new { message = "Unable to get character." });
            }
        }

        // POST /api/character/add/{id}
        [HttpPost("add/{id:int}")]
        [Authorize]
        public async Task<IActionResult> AddCharacter(int id)
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

                var character = await _context.Characters.FindAsync(id);
                if (character == null)
                {
                    return NotFound(new { message = "Character not found." });
                }

                // TODO : check if user already owns character

                if (user.Balance < character.Price)
                {
                    return StatusCode(403, new { message = "Not enough funds." });
                }

                var characterInstance = new CharacterInstance
                {
                    Character = character,
                    UserId = user.Id,
                    AttackStat = character.BaseAttackStat,
                    SpeedStat = character.BaseSpeedStat,
                    CritRateStat = character.BaseCritRateStat,
                    CritDamageStat = character.BaseCritDamageStat,
                    EnergyRechargeStat = character.BaseEnergyRechargeStat
                };

                user.Balance -= character.Price;
                user.Characters.Add(characterInstance);

                await _context.SaveChangesAsync();

                return Ok(characterInstance);
            } catch (Exception e) {
                _logger.LogError(e, "Error in CharacterController - AddCharacter:");
                return StatusCode(500, new { message = "Unable to add character." });
            }
        }

        // PATCH /api/character/level/{id}
        [HttpPatch("level/{id:int}")]
        [Authorize]
        public async Task<IActionResult> LevelCharacter(int id)
        {
            try {
                var email = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest("Invalid email claim.");
                }

                var user = await _context.Users.Include(u => u.Characters)
                                            .ThenInclude(ci => ci.Character)  // TODO: consider not eager loading
                                            .FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                if (user.Exp < 1)
                {
                    return StatusCode(403, new { message = "Not enough material." });
                }

                var characterInstance = user.Characters.FirstOrDefault(ci => ci.Character.Id == id);
                if (characterInstance == null)
                {
                    return NotFound(new { message = "Character not found." });
                }

                // TODO: fix calculations and hardcoded values
                user.Exp -= 1;
                characterInstance.Experience += 50;
                if (characterInstance.Experience >= 100) {
                    characterInstance.Level += characterInstance.Experience / 100;
                    characterInstance.Experience %= 100;
                }

                _context.Users.Update(user);
                _context.CharacterInstances.Update(characterInstance);

                await _context.SaveChangesAsync();

                return Ok(characterInstance);
            } catch (Exception e) {
                _logger.LogError(e, "Error in CharacterController - LevelCharacter:");
                return StatusCode(500, new { message = "Unable to level character." });
            }
        }
    }
}
