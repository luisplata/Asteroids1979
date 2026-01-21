using Unity.VisualScripting;
using UnityEngine;

public class ExplosionOnKill : MonoBehaviour
{
    [SerializeField] private float radius = 2f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private GameObject explosionVfx;
    [SerializeField] private bool isChained;

    public void Trigger(Vector2 position)
    {
        if (explosionVfx != null)
        {
            var vfx = Instantiate(explosionVfx, position, Quaternion.identity);
            vfx.transform.localScale = new Vector3(radius, radius, radius);
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(position, radius);

        foreach (var hit in hits)
        {
            var asteroid = hit.GetComponent<Asteroid>();
            if (asteroid != null && !asteroid.StartExplotion)
            {
                if (isChained)
                {
                    if (!asteroid.TryGetComponent(out ExplosionFromKiller fromKiller))
                    {
                        fromKiller = asteroid.AddComponent<ExplosionFromKiller>();
                    }

                    fromKiller.Configure(radius, damage, explosionVfx, isChained);
                }

                asteroid.TakeDamage(damage);
            }
        }
    }

    public void Configure(float _radius, float _damage, GameObject _vfx, bool _isChained)
    {
        radius = _radius;
        damage = _damage;
        explosionVfx = _vfx;
        isChained = _isChained;
    }
}