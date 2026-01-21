using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private TMP_Text healthText, damage, speed, score, level;

    private void Awake()
    {
        player.onPlayerStatsUpdated += UpdateHUD;
        ScoreSystem.Instance.OnLevelUp += UpdateLevel;
        ScoreSystem.Instance.OnScoreChange += UpdateScore;
    }

    private void UpdateScore(int _score, int nextLevel)
    {
        score.text = $"{_score}/{nextLevel}";
    }

    private void UpdateLevel(int obj)
    {
        level.text = $"{obj}";
    }

    private void UpdateHUD(PlayerStats obj)
    {
        healthText.text = $"{obj.currentHealth}";
        damage.text = $"{obj.damage}";
        speed.text = $"{obj.fireRate}";
    }

    private void OnDisable()
    {
        player.onPlayerStatsUpdated -= UpdateHUD;
        ScoreSystem.Instance.OnLevelUp -= UpdateLevel;
        ScoreSystem.Instance.OnScoreChange -= UpdateScore;
    }
}