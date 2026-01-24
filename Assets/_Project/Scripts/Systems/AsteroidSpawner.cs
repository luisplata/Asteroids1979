using _Project.Scripts.Bootstrap;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidSpawner : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private GameObject asteroidPrefab;

    [SerializeField] private Transform player;

    [Header("Spawn Timing")] [SerializeField]
    private float spawnRate = 1.5f;

    [SerializeField] private float minSpawnRate = 0.4f;
    [SerializeField] private float difficultyRamp = 0.98f;

    [Header("Spawn Position")] [SerializeField]
    private float spawnDistance = 8f;

    [SerializeField] private float spawnDistanceVariance = 1f;

    [Header("Direction Cone")] [Tooltip("Cone angle in degrees")] [SerializeField]
    private float coneAngle = 30f;

    [Header("Asteroid Speed")] [SerializeField]
    private float minSpeedMultiplier = 0.8f;

    [SerializeField] private float maxSpeedMultiplier = 1.2f;

    private Vector3 lastSpawnPos;
    private Vector2 lastIdealDir;
    private bool hasSpawned;
    private Vector2 lastFinalDirection;

    private float timer;

    private bool _isGameplay;
    
    public void Configure()
    {
        GameBootstrap.Instance.GameState.OnGameStarted += GameStateOnOnGameStarted;
        GameBootstrap.Instance.GameState.OnGameOver += GameStateOnOnGameOver;
    }

    private void OnDisable()
    {
        GameBootstrap.Instance.GameState.OnGameStarted -= GameStateOnOnGameStarted;
        GameBootstrap.Instance.GameState.OnGameOver -= GameStateOnOnGameOver;
    }

    private void GameStateOnOnGameOver()
    {
        _isGameplay = false;
    }

    private void GameStateOnOnGameStarted()
    {
        _isGameplay = true;
    }

    void Update()
    {
        if (!_isGameplay) return;
        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            timer = 0f;
            Spawn();
        }
    }

    void Spawn()
    {
        if (player == null) return;

        // 1️⃣ Posición de spawn (fuera de cámara)
        Vector2 spawnDir = Random.insideUnitCircle.normalized;
        float distance = spawnDistance + Random.Range(-spawnDistanceVariance, spawnDistanceVariance);
        Vector3 spawnPos = player.position + (Vector3)(spawnDir * distance);

        // 2️⃣ Dirección ideal hacia el player
        Vector2 idealDir = (player.position - spawnPos).normalized;

        // 3️⃣ Variación angular (el "fallo")
        float halfCone = coneAngle * 0.5f;
        float angleOffset = Random.Range(-halfCone, halfCone);

        Vector2 finalDirection =
            Quaternion.AngleAxis(angleOffset, Vector3.forward) * idealDir;

        // 4️⃣ Instanciar
        GameObject asteroid = Instantiate(asteroidPrefab, spawnPos, Quaternion.identity);

        // 5️⃣ Aplicar movimiento
        var asteroidComponent = asteroid.GetComponent<Asteroid>();
        if (asteroidComponent != null)
        {
            float speedMultiplier = Random.Range(minSpeedMultiplier, maxSpeedMultiplier);
            asteroidComponent.SetMoveDirection(finalDirection, speedMultiplier);
        }

        // 6️⃣ Escalado dificultad
        spawnRate = Mathf.Max(minSpawnRate, spawnRate * difficultyRamp);

        // Cache para gizmos
        lastSpawnPos = spawnPos;
        lastIdealDir = idealDir;
        lastFinalDirection = finalDirection;
        hasSpawned = true;
    }
#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (!hasSpawned || player == null)
            return;

        float debugLength = 4f;

        // 🔴 Punto de spawn
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(lastSpawnPos, 0.15f);

        // 🟢 Dirección FINAL (trayectoria real)
        Gizmos.color = Color.green;
        Gizmos.DrawLine(
            lastSpawnPos,
            lastSpawnPos + (Vector3)lastFinalDirection * debugLength
        );

        // 🟡 Dirección ideal al player
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(
            lastSpawnPos,
            lastSpawnPos + (Vector3)lastIdealDir * debugLength
        );

        // 🟡 Cono
        float halfCone = coneAngle * 0.5f;

        Vector3 leftDir =
            Quaternion.AngleAxis(-halfCone, Vector3.forward) * lastIdealDir;

        Vector3 rightDir =
            Quaternion.AngleAxis(halfCone, Vector3.forward) * lastIdealDir;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(lastSpawnPos, lastSpawnPos + leftDir * debugLength);
        Gizmos.DrawLine(lastSpawnPos, lastSpawnPos + rightDir * debugLength);

        // 🔵 Línea al player (referencia)
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(lastSpawnPos, player.position);
    }
#endif
}