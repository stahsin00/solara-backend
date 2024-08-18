using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CharacterController : ControllerBase
{
    private readonly List<Character> _characters = new List<Character>
    {
        new Character { Id = 1, Name = "Warrior", BaseAttackStat = 50, BaseSpeedStat = 10, BaseCritRateStat = 0.05f, BaseCritDamageStat = 1.5f, BaseEnergyRechargeStat = 1.0f },
        new Character { Id = 2, Name = "Mage", BaseAttackStat = 30, BaseSpeedStat = 12, BaseCritRateStat = 0.03f, BaseCritDamageStat = 1.2f, BaseEnergyRechargeStat = 1.1f },
        new Character { Id = 3, Name = "Archer", BaseAttackStat = 45, BaseSpeedStat = 14, BaseCritRateStat = 0.07f, BaseCritDamageStat = 1.4f, BaseEnergyRechargeStat = 1.2f }
    };

    // GET /api/character
    [HttpGet]
    public IActionResult GetAllCharacters()
    {
        return Ok(_characters);
    }

    // GET /api/character/{id}
    [HttpGet("{id:int}")]
    public IActionResult GetCharacterById(int id)
    {
        var character = _characters.FirstOrDefault(c => c.Id == id);
        return character is not null ? Ok(character) : NotFound();
    }
}
