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

        // TODO: use an array
        public int? Teamcharacter1Id { get; set; }
        public CharacterInstance? TeamCharacter1 { get; set; }

        public int? Teamcharacter2Id { get; set; }
        public CharacterInstance? TeamCharacter2 { get; set; }
        
        public int? Teamcharacter3Id { get; set; }
        public CharacterInstance? TeamCharacter3 { get; set; }
        
        public int? Teamcharacter4Id { get; set; }
        public CharacterInstance? TeamCharacter4 { get; set; }

        public ICollection<Quest> Quests { get; set; } = new List<Quest>();
        public ICollection<CharacterInstance> Characters { get; set; } = new List<CharacterInstance>();
        // public ICollection<EquipmentInstance> Equipments { get; set; } = new List<EquipmentInstance>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastLogin { get; set; } = DateTime.UtcNow;
    }
}
