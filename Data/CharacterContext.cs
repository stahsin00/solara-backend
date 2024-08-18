using Microsoft.EntityFrameworkCore;
using Solara.Models;

namespace Solara.Data
{
    public class CharacterContext : DbContext
    {
        public CharacterContext(DbContextOptions<CharacterContext> options) : base(options) { }

        public DbSet<Character> Characters { get; set; } = null!;
    }
}
