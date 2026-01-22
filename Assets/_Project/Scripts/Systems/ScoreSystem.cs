using System;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    private static ScoreSystem _instance;
    private static bool _isShuttingDown = false;

    // Lazily find or create an instance when needed at runtime.
    public static ScoreSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                // If quitting or not in play mode, don't create an instance
                if (_isShuttingDown || !Application.isPlaying)
                    return null;

                // Try to find an existing instance in the scene (use FindFirstObjectByType if available)
                _instance = FindFirstObjectByType<ScoreSystem>();

                if (_instance != null)
                    return _instance;

                // Create a new GameObject with the ScoreSystem component
                var go = new GameObject("ScoreSystem");
                _instance = go.AddComponent<ScoreSystem>();
                DontDestroyOnLoad(go);
            }

            return _instance;
        }
    }

    public int Score => score;

    public Action<int> OnLevelUp;
    public Action<int, int> OnScoreChange;

    [SerializeField] private int score;
    [SerializeField] private int level = 1;
    [SerializeField] private int nextLevelScoreThreshold = 10;
    [SerializeField] private int increase = 2;

    private void OnEnable()
    {
        OnScoreChange?.Invoke(score, nextLevelScoreThreshold);
        OnLevelUp?.Invoke(level);
    }

    private void OnApplicationQuit()
    {
        // Mark that the application is quitting to avoid creating new instances during shutdown
        _isShuttingDown = true;
    }

    private void OnDestroy()
    {
        // Prevent other OnDestroy calls from creating a new instance while scene is unloading
        _isShuttingDown = true;

        // Clear the static reference if this instance is being destroyed
        if (_instance == this)
        {
            _instance = null;
        }
    }

    public void Add(int amount)
    {
        score += amount;
        OnScoreChange?.Invoke(score, nextLevelScoreThreshold);
        if (score >= nextLevelScoreThreshold)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        increase = 2;
        nextLevelScoreThreshold += level * increase; // Increase threshold for next level
        Debug.Log("Leveled Up! New Level: " + level);
        OnLevelUp?.Invoke(level);
    }
}