using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private AsteroidStats stats;
    [SerializeField] private float lifeTime;

    private float currentHealth;
    private Vector2 moveDirection;

    private bool _isDead = false;

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

    public bool TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0f && !_isDead)
        {
            Debug.Log("Asteroid destroyed");
            Die();
            ScoreSystem.Instance?.Add(1);
            _isDead = true;
            return true;
        }

        return false;
    }

    private void Die(float timeToDelay = 0.1f)
    {
        Destroy(gameObject, timeToDelay);
    }

    public float CollisionDamage()
    {
        return stats.damageOnHit;
    }
}