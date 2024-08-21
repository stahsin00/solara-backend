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

        // POST /api/character/addcharacter/{id}
        [HttpPost("addcharacter/{id:int}")]
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
                    AttackStat = character.BaseAttackStat,
                    SpeedStat = character.BaseSpeedStat,
                    CritRateStat = character.BaseCritRateStat,
                    CritDamageStat = character.BaseCritDamageStat,
                    EnergyRechargeStat = character.BaseEnergyRechargeStat
                };

                user.Balance -= character.Price;
                user.Characters.Add(characterInstance);

                await _context.CharacterInstances.AddAsync(characterInstance);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Character added." });
            } catch (Exception e) {
                _logger.LogError(e, "Error in CharacterController - AddCharacter:");
                return StatusCode(500, new { message = "Unable to add character." });
            }
        }
    }
}
