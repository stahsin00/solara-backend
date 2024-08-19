using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Solara.Data;

namespace Solara.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly CharacterContext _context;

        public CharacterController(CharacterContext context)
        {
            _context = context;
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
    }
}
