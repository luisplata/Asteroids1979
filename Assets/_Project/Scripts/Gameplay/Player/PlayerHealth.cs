using _Project.Scripts.Bootstrap;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 5f;
    [SerializeField] private float currentHealth;

    public void Configure()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Game Over");
        GameBootstrap.Instance.GameState.GameOver();
    }
}