using UnityEngine;

public class ExplosionOnKillProvider : MonoBehaviour
{
    private float _radius;
    private float _damage;
    private GameObject _vfx;
    private bool _isChained;

    public void Init(float radius, float damage, GameObject vfx)
    {
        _radius = radius;
        _damage = damage;
        _vfx = vfx;
    }

    public void EnableChained()
    {
        _isChained = true;
    }

    public void DisableChained()
    {
        _isChained = false;
    }

    public void ApplyTo(GameObject bullet)
    {
        var explosion = bullet.AddComponent<ExplosionOnKill>();
        explosion.Configure(_radius, _damage, _vfx, _isChained);
    }

    public void Increment(float radius, float damage)
    {
        _radius += radius / 2;
        _damage += damage;
    }
}