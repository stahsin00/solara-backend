namespace Solara.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string ProfilePicture { get; set; } = "https://www.gravatar.com/avatar/00000000000000000000000000000000?d=mp&f=y&s=200"; // TODO
        public string Email { get; set; } = null!;
        
        public string OAuthProvider { get; set; } = null!;
        public string OAuthProviderUserId { get; set; } = null!;

        public int Balance { get; set; } = 5000; // TODO: centralize hardcoded values
        public int Exp { get; set; } = 10; // TODO

        public int? TeamCharacter1Id { get; set; }
        public int? TeamCharacter2Id { get; set; }
        public int? TeamCharacter3Id { get; set; }
        public int? TeamCharacter4Id { get; set; }

        public ICollection<Quest> Quests { get; set; } = new List<Quest>();
        public ICollection<CharacterInstance> Characters { get; set; } = new List<CharacterInstance>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastLogin { get; set; } = DateTime.UtcNow;
    }
}
