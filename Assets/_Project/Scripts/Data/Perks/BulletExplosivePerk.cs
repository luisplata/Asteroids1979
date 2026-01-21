using UnityEngine;

[CreateAssetMenu(menuName = "Perks/Bullet/Explosive")]
public class BulletExplosivePerk : PerkData
{
    [SerializeField] private float radius = 2f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private GameObject vfx;

    public override void Apply(Player player)
    {
        player.bulletConfig.explosive = true;

        if (player.gameObject.TryGetComponent<ExplosionOnKillProvider>(out var explotion))
        {
            explotion.Increment(radius, damage);
        }
        else
        {
            player.gameObject.AddComponent<ExplosionOnKillProvider>()
                .Init(radius, damage, vfx);
        }
        
        isSelectable = false;
    }
}