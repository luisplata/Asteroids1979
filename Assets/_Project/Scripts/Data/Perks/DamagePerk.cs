using UnityEngine;

[CreateAssetMenu(menuName = "Perks/Damage Up")]
public class DamagePerk : PerkData
{
    [SerializeField] private float multiplier = 1.25f;

    public override void Apply(Player player)
    {
        player.stats.damage *= multiplier;
        player.UpdateStats();
    }
}