using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerHealth health;
    public PlayerStats stats;

    void Awake()
    {
        stats.Init();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var asteroid = other.GetComponent<Asteroid>();
        if (asteroid != null)
        {
            health.TakeDamage(asteroid.CollisionDamage());
            return;
        }

        // var perk = other.GetComponent<Perk>();
        // if (perk != null)
        // {
        //     perk.ApplyTo(this);
        //     Destroy(perk.gameObject);
        //     return;
        // }
    }

    [ContextMenu("Restart Game")]
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}