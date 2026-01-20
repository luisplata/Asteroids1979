using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private AsteroidStats stats;
    [SerializeField] private float lifeTime;

    private float currentHealth;
    private Vector2 moveDirection;

    void Start()
    {
        currentHealth = stats.maxHealth;
        moveDirection = Random.insideUnitCircle.normalized;
        Die(lifeTime);
    }

    void Update()
    {
        transform.Translate(moveDirection * (stats.moveSpeed * Time.deltaTime));
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0f)
        {
            Die(0.1f);
        }
    }

    void Die(float timeToDelay)
    {
        Destroy(gameObject, timeToDelay);
    }
}