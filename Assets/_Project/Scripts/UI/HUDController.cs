using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private TMP_Text healthText, damage, speed, score, level;

    private void Start()
    {
        if (player != null)
            player.onPlayerStatsUpdated += UpdateHUD;

        if (ScoreSystem.Instance != null)
        {
            ScoreSystem.Instance.OnLevelUp += UpdateLevel;
            ScoreSystem.Instance.OnScoreChange += UpdateScore;
            UpdateLevel(1);
            UpdateScore(0,0);
        }
    }

    private void UpdateScore(int _score, int nextLevel)
    {
        if (score != null)
            score.text = $"{_score}/{nextLevel}";
    }

    private void UpdateLevel(int obj)
    {
        if (level != null)
            level.text = $"{obj}";
    }

    private void UpdateHUD(PlayerStats obj)
    {
        if (healthText != null)
            healthText.text = $"{obj.currentHealth}";
        if (damage != null)
            damage.text = $"{obj.damage}";
        if (speed != null)
            speed.text = $"{obj.fireRate}";
    }

    private void OnDisable()
    {
        if (player != null)
            player.onPlayerStatsUpdated -= UpdateHUD;

        if (ScoreSystem.Instance != null)
        {
            ScoreSystem.Instance.OnLevelUp -= UpdateLevel;
            ScoreSystem.Instance.OnScoreChange -= UpdateScore;
        }
    }
}