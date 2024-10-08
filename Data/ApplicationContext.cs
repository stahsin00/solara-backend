using Microsoft.EntityFrameworkCore;
using Solara.Models;

namespace Solara.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;

        public DbSet<Quest> Quests { get; set; } = null!;

        public DbSet<Character> Characters { get; set; } = null!;
        public DbSet<CharacterInstance> CharacterInstances { get; set; } = null!;
        
        public DbSet<EquipmentInstance> EquipmentInstances { get; set; } = null!;

        public DbSet<Game> Games { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Quest>()
                .HasDiscriminator<string>("Type")
                .HasValue<Quest>("Regular")
                .HasValue<RecurrentQuest>("Recurrent");

            modelBuilder.Entity<User>()
                .HasOne(u => u.TeamCharacter1)
                .WithMany()  // for one sided relationships even though it's not actually one-to-many
                .HasForeignKey(u => u.Teamcharacter1Id)
                .OnDelete(DeleteBehavior.SetNull);
            
            modelBuilder.Entity<User>()
                .HasOne(u => u.TeamCharacter2)
                .WithMany()
                .HasForeignKey(u => u.Teamcharacter2Id)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<User>()
                .HasOne(u => u.TeamCharacter3)
                .WithMany()
                .HasForeignKey(u => u.Teamcharacter3Id)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<User>()
                .HasOne(u => u.TeamCharacter4)
                .WithMany()
                .HasForeignKey(u => u.Teamcharacter4Id)
                .OnDelete(DeleteBehavior.SetNull);
            
            modelBuilder.Entity<Character>().HasData(
                new Character
                {
                    Id = 1,
                    Name = "Margana",
                    FullArt = "art/margana.png",
                    FaceArt = "art/margana-face.png",
                    Description = "A laid-back wanderer drifting through Solara.",
                    Element = "Blue",
                    Price = 5000,
                    BaseAttackStat = 100,
                    BaseSpeedStat = 20,
                    BaseCritRateStat = 0.1f,
                    BaseCritDamageStat = 1.5f,
                    BaseEnergyRechargeStat = 1.0f,
                    BasicAttack = "Sword Slash",
                    SpecialAttack = "Blazing Strike"
                },
                new Character
                {
                    Id = 2,
                    Name = "Artemisia",
                    FullArt = "art/a.png",
                    FaceArt = "art/a-face.png",
                    Description = "A passionate artist known to get fired up.",
                    Element = "Magenta",
                    Price = 5000,
                    BaseAttackStat = 80,
                    BaseSpeedStat = 15,
                    BaseCritRateStat = 0.12f,
                    BaseCritDamageStat = 1.4f,
                    BaseEnergyRechargeStat = 1.2f,
                    BasicAttack = "Water Blast",
                    SpecialAttack = "Tsunami"
                },
                new Character
                {
                    Id = 3,
                    Name = "Powder",
                    FullArt = "art/b.png",
                    FaceArt = "art/b-face.png",
                    Description = "A hardworking maid just trying her best.",
                    Element = "Magenta",
                    Price = 5000,
                    BaseAttackStat = 80,
                    BaseSpeedStat = 15,
                    BaseCritRateStat = 0.12f,
                    BaseCritDamageStat = 1.4f,
                    BaseEnergyRechargeStat = 1.2f,
                    BasicAttack = "Water Blast",
                    SpecialAttack = "Tsunami"
                },
                new Character
                {
                    Id = 4,
                    Name = "Ann",
                    FullArt = "art/e.png",
                    FaceArt = "art/e-face.png",
                    Description = "A medical student currently enrolled at the University of Solara.",
                    Element = "Magenta",
                    Price = 5000,
                    BaseAttackStat = 80,
                    BaseSpeedStat = 15,
                    BaseCritRateStat = 0.12f,
                    BaseCritDamageStat = 1.4f,
                    BaseEnergyRechargeStat = 1.2f,
                    BasicAttack = "Water Blast",
                    SpecialAttack = "Tsunami"
                }
            );
        }
    }
}
