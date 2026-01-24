using UnityEngine;
using _Project.Scripts.Gameplay;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Bootstrap
{
    public class GameBootstrap : MonoBehaviour
    {
        public static GameBootstrap Instance { get; private set; }

        public GameStateMachine GameState;
        public TimeScaleGuard TimeScale;

        [SerializeField] private AsteroidSpawner asteroidSpawner;
        [SerializeField] private Player player;
        [SerializeField] private GameObject gameOverScreen;

        void Awake()
        {
            if (Instance != null)
            {
                // Destroy(gameObject);
                return;
            }

            Instance = this;
            // DontDestroyOnLoad(gameObject);
            HookStateEvents();
            asteroidSpawner.Configure();
            player.Configure();
        }

        void HookStateEvents()
        {
            GameState.OnGameStarted += TimeScale.Resume;
            GameState.OnReturnedToMenu += TimeScale.Resume;
            GameState.OnGameOver += TimeScale.Pause;
            GameState.OnGameOver += () => { gameOverScreen.SetActive(true); };
        }
        
        public void RestartGame()
        {
            SceneManager.LoadScene(0);
        }
    }
}