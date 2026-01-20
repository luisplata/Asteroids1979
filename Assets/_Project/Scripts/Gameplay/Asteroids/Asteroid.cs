using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private AsteroidStats stats;
    [SerializeField] private float lifeTime;
    private float _speedMultiplier = 1f;

    private float _currentHealth;
    private Vector2 _moveDirection;

    private bool _isDead = false;

    void Start()
    {
        _currentHealth = stats.maxHealth;
        Die(lifeTime);
    }

    void Update()
    {
        transform.Translate(_moveDirection * (stats.moveSpeed * Time.deltaTime));
    }

    public bool TakeDamage(float amount)
    {
        _currentHealth -= amount;

        if (_currentHealth <= 0f && !_isDead)
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

    public void SetMoveDirection(Vector2 direction, float speedMultiplier)
    {
        _moveDirection = direction.normalized;
        _speedMultiplier = speedMultiplier;
    }
}