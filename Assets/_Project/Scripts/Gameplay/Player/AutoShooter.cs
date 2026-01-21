using UnityEngine;

public class AutoShooter : MonoBehaviour
{
    [Header("Shooting")] [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate = 2f;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float maxTargetDistance = 10f;

    private Player player;

    private float timer;

    void Awake()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1f / player.stats.fireRate)
        {
            timer = 0f;
            ShootAtClosestAsteroid();
        }
    }

    void ShootAtClosestAsteroid()
    {
        Asteroid target = FindClosestAsteroid();
        if (target == null) return;

        Vector2 direction =
            (target.transform.position - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle - 90f);

        var bullet = Instantiate(
            bulletPrefab,
            transform.position,
            rotation
        );

        var provider = player.GetComponent<ExplosionOnKillProvider>();
        provider?.ApplyTo(bullet);

        var rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * (bulletSpeed * player.bulletConfig.speedMultiplier);

        var bulletDamate = bullet.GetComponent<BulletDamage>();
        bulletDamate.SetDamage(player.stats.damage * player.bulletConfig.damage);
        bulletDamate.ConfigurePierce(
            player.bulletConfig.pierce,
            player.bulletConfig.pierceCount
        );
    }


    Asteroid FindClosestAsteroid()
    {
        Asteroid[] asteroids = FindObjectsByType<Asteroid>(
            FindObjectsSortMode.None
        );

        Asteroid closest = null;
        float minDistance = maxTargetDistance;

        foreach (var asteroid in asteroids)
        {
            float dist = Vector2.Distance(
                transform.position,
                asteroid.transform.position
            );

            if (dist < minDistance)
            {
                minDistance = dist;
                closest = asteroid;
            }
        }

        return closest;
    }
}