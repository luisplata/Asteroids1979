using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerHealth health;
    public PlayerStats stats;
    public BulletConfig bulletConfig;

    // New: store whether player wants chain explosions (set by perks)
    public bool chainExplosions = false;

    public Action<PlayerStats> onPlayerStatsUpdated;

    void Awake()
    {
        stats.Init();
    }

    private void OnEnable()
    {
        UpdateStats();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var asteroid = other.GetComponent<Asteroid>();
        if (asteroid != null)
        {
            health.TakeDamage(asteroid.CollisionDamage());
        }
    }

    [ContextMenu("Restart Game")]
    public void RestartGame()
    {
        GamePauseController.Resume();
    }

    public void UpdateStats()
    {
        onPlayerStatsUpdated?.Invoke(stats);
    }
}