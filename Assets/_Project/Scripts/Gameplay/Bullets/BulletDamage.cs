using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    private float damage;
    private ExplosionOnKill explosionEffect;
    [SerializeField] private bool pierce;
    [SerializeField] private int maxPierceCount = 1;
    private int currentPierce;

    public void ConfigurePierce(bool enabled, int count = 1)
    {
        pierce = enabled;
        maxPierceCount = count;
    }

    public void SetDamage(float value)
    {
        damage = value;
        explosionEffect = GetComponent<ExplosionOnKill>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var asteroid = other.GetComponent<Asteroid>();
        if (asteroid == null) return;

        if (asteroid.TakeDamage(damage))
        {
            OnAsteroidKilled(asteroid);
        }

        if (!pierce || currentPierce >= maxPierceCount)
        {
            Destroy(gameObject);
        }
        else
        {
            currentPierce++;
        }
    }

    void OnAsteroidKilled(Asteroid asteroid)
    {
        Debug.Log("Asteroid Killed");
        explosionEffect?.Trigger(asteroid.transform.position);
    }
}