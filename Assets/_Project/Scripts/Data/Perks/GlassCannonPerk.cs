using UnityEngine;

[CreateAssetMenu(menuName = "Perks/Glass Cannon")]
public class GlassCannonPerk : PerkData
{
    [SerializeField] private float damageMultiplier = 1.5f;
    [SerializeField] private float healthMultiplier = 0.7f;

    public override void Apply(Player player)
    {
        player.stats.damage *= damageMultiplier;
        player.stats.maxHealth *= healthMultiplier;

        player.stats.currentHealth =
            Mathf.Min(player.stats.currentHealth, player.stats.maxHealth);
        player.UpdateStats();
    }
}