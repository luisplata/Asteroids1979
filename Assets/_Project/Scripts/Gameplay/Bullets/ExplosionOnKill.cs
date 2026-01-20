using UnityEngine;

public class ExplosionOnKill : MonoBehaviour
{
    [SerializeField] private float radius = 2f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private GameObject explosionVfx;

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
            if (asteroid != null)
            {
                asteroid.TakeDamage(damage);
            }
        }
    }

    public void Configure(float _radius, float _damage, GameObject _vfx)
    {
        radius = _radius;
        damage = _damage;
        explosionVfx = _vfx;
    }
}