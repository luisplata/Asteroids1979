using UnityEngine;

[CreateAssetMenu(menuName = "Perks/Tank")]
public class TankPerk : PerkData
{
    [SerializeField] private float healthMultiplier = 1.5f;
    [SerializeField] private float fireRateMultiplier = 0.85f;

    public override void Apply(Player player)
    {
        player.stats.maxHealth *= healthMultiplier;
        player.stats.fireRate *= fireRateMultiplier;

        player.stats.currentHealth = player.stats.maxHealth;
        player.UpdateStats();
    }
}