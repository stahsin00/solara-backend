namespace Solara.Models
{
    public class Game
    {
        public int Id { get; set; }
        public User User { get; set; } = null!;
        public Quest Quest { get; set; } = null!;

        public TimeSpan RemainingTime { get; set; }

        // TODO: ignore for now, enemy types has not been implemented yet, just using basic health for now
        public float EnemyCurHealth { get; set; }
        public float EnemyMaxHealth { get; set; }

        public float DPS { get; set; }
        public float CritRate  { get; set; }
        public float CritDamage { get; set; }
        // TODO: ignore for now, keep the characters currently in the game for special attacks or other character specific calculations

        public bool Running { get; set; } = true;

        public int RewardBalance { get; set; } = 0;
        public int RewardExp { get; set; } = 0;
        // TODO: ignore for now, equipment feature has not yet been implemented
        // public ICollection<EquipmentInstance> RewardEquipments { get; set; } = new List<EquipmentInstance>();

        public DateTime StartTime { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
    }
}
