using Microsoft.EntityFrameworkCore;
using Solara.Models;

namespace Solara.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
    }
}
