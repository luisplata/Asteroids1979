using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private float spawnRate = 1.5f;
    [SerializeField] private float spawnRadius = 7f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            timer = 0f;
            Spawn();
        }
    }

    void Spawn()
    {
        Vector2 dir = Random.insideUnitCircle.normalized;
        Vector3 pos = dir * spawnRadius;

        Instantiate(asteroidPrefab, pos, Quaternion.identity);
    }
}