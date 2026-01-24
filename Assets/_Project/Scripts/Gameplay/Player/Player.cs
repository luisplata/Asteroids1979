using System;
using _Project.Scripts.Bootstrap;
using _Project.Scripts.Gameplay.Player;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerHealth health;
    public PlayerStats stats;
    public BulletConfig bulletConfig;
    [SerializeField] private PlayerController controller;
    [SerializeField] private AutoShooter autoShooter;

    public Action onShoot;
    public Action onLevelUp;
    

    // New: store whether player wants chain explosions (set by perks)
    public bool chainExplosions = false;

    public Action<PlayerStats> onPlayerStatsUpdated;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var asteroid = other.GetComponent<Asteroid>();
        if (asteroid != null)
        {
            health.TakeDamage(asteroid.CollisionDamage());
            AudioPlayerController.Instance.PlayPlayerHitSound();
        }
    }

    [ContextMenu("Restart Game")]
    public void RestartGame()
    {
        GameBootstrap.Instance.TimeScale.Resume();
    }

    public void UpdateStats()
    {
        onPlayerStatsUpdated?.Invoke(stats);
    }

    public void Configure()
    {
        stats.Init();
        UpdateStats();
        health.Configure();
        controller.Configure();
        autoShooter.Configure(this);
    }
}