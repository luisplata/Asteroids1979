using UnityEngine;

[CreateAssetMenu(menuName = "Perks/Bullet/Damage")]
public class BulletDamagePerk : PerkData
{
    [SerializeField] private float multiplier = 1.25f;

    public override void Apply(Player player)
    {
        player.bulletConfig.damage *= multiplier;
    }
}