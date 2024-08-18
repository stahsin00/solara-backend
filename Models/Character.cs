public class Character
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string FullArt { get; set; } = null!;
    public string FaceArt { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Element { get; set; } = null!;

    public int BaseAttackStat { get; set; }
    public int BaseSpeedStat { get; set; }
    public float BaseCritRateStat { get; set; }
    public float BaseCritDamageStat { get; set; }
    public float BaseEnergyRechargeStat { get; set; }
    
    public string BasicAttack { get; set; } = null!;
    public string SpecialAttack { get; set; } = null!;
}
