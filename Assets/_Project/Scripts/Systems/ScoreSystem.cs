using System;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    private static ScoreSystem _instance;

    public static ScoreSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ScoreSystem>();
                if (_instance != null) return _instance;
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