using Unity.VisualScripting;
using UnityEngine;

public class ExplosionFromKiller : MonoBehaviour
{
    private float _radius;
    private float _damage;
    private GameObject _explosionVfx;
    private bool _isChained;

    public void Configure(float radius, float damage, GameObject explosionVfx, bool isChained)
    {
        _radius = radius;
        _damage = damage;
        _explosionVfx = explosionVfx;
        _isChained = isChained;
    }

    public void Trigger(Vector3 position)
    {
        if (_explosionVfx != null)
        {
            var vfx = Instantiate(_explosionVfx, position, Quaternion.identity);
            vfx.transform.localScale = new Vector3(_radius, _radius, _radius);
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(position, _radius);

        foreach (var hit in hits)
        {
            var asteroid = hit.GetComponent<Asteroid>();
            if (asteroid != null && !asteroid.StartExplotion)
            {
                // Solo propagar la cadena si esta instancia tiene permiso para hacerlo
                if (_isChained)
                {
                    // Evitar añadir componentes duplicados
                    if (!asteroid.TryGetComponent<ExplosionFromKiller>(out var existing))
                    {
                        var fromKiller = asteroid.AddComponent<ExplosionFromKiller>();
                        // Las nuevas instancias no propagarán más (rompe el ciclo)
                        fromKiller.Configure(_radius, _damage, _explosionVfx, false);
                    }
                }

                asteroid.TakeDamage(_damage);
            }
        }
    }
}