using UnityEngine;

public class ExplosionOnKillProvider : MonoBehaviour
{
    private float radius;
    private float damage;
    private GameObject vfx;

    public void Init(float radius, float damage, GameObject vfx)
    {
        this.radius = radius;
        this.damage = damage;
        this.vfx = vfx;
    }

    public void ApplyTo(GameObject bullet)
    {
        var explosion = bullet.AddComponent<ExplosionOnKill>();
        explosion.Configure(radius, damage, vfx);
    }
}