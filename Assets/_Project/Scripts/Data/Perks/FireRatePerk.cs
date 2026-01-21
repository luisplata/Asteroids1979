using UnityEngine;

[CreateAssetMenu(menuName = "Perks/Fire Rate")]
public class FireRatePerk : PerkData
{
    [SerializeField] private float fireRateMultiplier = 1.25f;

    public override void Apply(Player player)
    {
        player.stats.fireRate *= fireRateMultiplier;
        player.UpdateStats();
    }
}