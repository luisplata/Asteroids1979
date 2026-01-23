using UnityEngine;

[CreateAssetMenu(menuName = "Perks/Bullet/ChainExplosionPerk")]
public class BulletChainExplosionPerk : PerkData
{
    public override void Apply(Player target)
    {
        // Store preference on player so future provider will inherit it
        target.chainExplosions = true;

        target.TryGetComponent<ExplosionOnKillProvider>(out var explosionFromKiller);
        if (explosionFromKiller != null)
        {
            explosionFromKiller.EnableChained();
        }

        isSelectable = false;
        target.UpdateStats();
    }
}