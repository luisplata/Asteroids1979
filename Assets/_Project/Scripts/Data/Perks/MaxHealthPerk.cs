using UnityEngine;

[CreateAssetMenu(menuName = "Perks/Max Health Up")]
public class MaxHealthPerk : PerkData
{
    [SerializeField] private float healthIncrease = 1f;

    public override void Apply(Player player)
    {
        player.stats.maxHealth += healthIncrease;
        player.stats.currentHealth += healthIncrease;
        player.UpdateStats();
    }
}