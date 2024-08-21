namespace Solara.Models
{
    public class EquipmentInstance
    {
        public int Id { get; set; }
        // public User User { get; set; } = null!;

        // public CharacterInstance? CharacterInstance { get; set; }

        public DateTime ObtainedAt { get; set; } = DateTime.UtcNow;
    }
}
