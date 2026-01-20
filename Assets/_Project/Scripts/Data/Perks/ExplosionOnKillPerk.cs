using UnityEngine;

[CreateAssetMenu(menuName = "Perks/Explosion On Kill")]
public class ExplosionOnKillPerk : PerkData
{
    [SerializeField] private float radius = 2f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private GameObject explosionVfx;

    public override void Apply(Player player)
    {
        player.gameObject.AddComponent<ExplosionOnKillProvider>()
            .Init(radius, damage, explosionVfx);
    }
}