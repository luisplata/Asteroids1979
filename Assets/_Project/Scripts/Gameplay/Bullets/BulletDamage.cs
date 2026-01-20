using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    private float damage;

    public void SetDamage(float value)
    {
        damage = value;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var asteroid = other.GetComponent<Asteroid>();
        if (asteroid == null) return;

        asteroid.TakeDamage(damage);
        Destroy(gameObject);
    }
}