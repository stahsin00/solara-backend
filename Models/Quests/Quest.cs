namespace Solara.Models
{
    public class Quest
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int UserId { get; set; }

        public DateTime? Deadline { get; set; }

        public Difficulty Difficulty { get; set; }
        public bool Important { get; set; } = false;

        public bool Complete { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
