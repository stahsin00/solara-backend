namespace Solara.Models
{
    public class CharacterInstance
    {
        public int Id { get; set; }

        public Character Character { get; set; } = null!;

        public int UserId { get; set; }

        public int Experience { get; set; } = 0;
        public int Level { get; set; } = 0;

        public float AttackStat { get; set; }
        public float SpeedStat { get; set; }
        public float CritRateStat { get; set; }
        public float CritDamageStat { get; set; }
        public float EnergyRechargeStat { get; set; }

        // TODO
        //public EquipmentInstance? Headgear { get; set; }
        //public EquipmentInstance? Armor { get; set; }
        //public EquipmentInstance? Footwear { get; set; }
        //public EquipmentInstance? Weapon { get; set; }

        public int TeamPos { get; set; } = 0;

        public DateTime ObtainedAt { get; set; } = DateTime.UtcNow;
    }
}
