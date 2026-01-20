using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    private float damage;
    private ExplosionOnKill explosionEffect;

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

        Destroy(gameObject);
    }

    void OnAsteroidKilled(Asteroid asteroid)
    {
        Debug.Log("Asteroid Killed");
        explosionEffect?.Trigger(asteroid.transform.position);
    }
}