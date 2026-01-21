using UnityEngine;

[CreateAssetMenu(menuName = "Perks/Bullet/ChainExplosionPerk")]
public class BulletChainExplosionPerk : PerkData
{
    public override void Apply(Player target)
    {
        target.TryGetComponent<ExplosionOnKillProvider>(out var explosionFromKiller);
        if (explosionFromKiller != null)
        {
            explosionFromKiller.EnableChained();
            isSelectable = false;
        }
        
        target.UpdateStats();
    }
}