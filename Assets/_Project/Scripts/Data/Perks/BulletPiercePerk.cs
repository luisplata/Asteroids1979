using UnityEngine;

[CreateAssetMenu(menuName = "Perks/Bullet/Pierce")]
public class BulletPiercePerk : PerkData
{
    [SerializeField] private int additionalPierceCount = 1;

    public override void Apply(Player player)
    {
        player.bulletConfig.pierce = true;
        player.bulletConfig.pierceCount += additionalPierceCount;
    }
}