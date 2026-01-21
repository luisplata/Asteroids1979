using UnityEngine;

[CreateAssetMenu(menuName = "Perks/Bullet/Speed")]
public class BulletSpeedPerk : PerkData
{
    [SerializeField] private float multiplier = 1.2f;

    public override void Apply(Player player)
    {
        player.bulletConfig.speedMultiplier *= multiplier;
    }
}

