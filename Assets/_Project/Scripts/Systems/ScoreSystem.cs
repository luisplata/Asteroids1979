using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem Instance { get; private set; }
    public int Score => score;

    [SerializeField] private int score;

    void Awake()
    {
        Instance = this;
    }

    public void Add(int amount)
    {
        score += amount;
    }
}