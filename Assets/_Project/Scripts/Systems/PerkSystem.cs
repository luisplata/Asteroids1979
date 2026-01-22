using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PerkSystem : MonoBehaviour
{
    [SerializeField] private PerkContainer perkContainer;
    [SerializeField] private PerkSelectionUI perkSelectionUI;
    [SerializeField] private PerkData[] availablePerks;
    [SerializeField] private List<PerkData> selectedPerks;

    void OnEnable()
    {
        // Protect against ScoreSystem.Instance being null during editor/teardown
        if (ScoreSystem.Instance != null)
            ScoreSystem.Instance.OnLevelUp += OnLevelUp;

        selectedPerks = new List<PerkData>();

        if (availablePerks == null) return;

        foreach (var availablePerk in availablePerks)
        {
            if (availablePerk != null)
                selectedPerks.Add(Instantiate(availablePerk));
        }
    }

    void OnDisable()
    {
        if (ScoreSystem.Instance != null)
            ScoreSystem.Instance.OnLevelUp -= OnLevelUp;

        selectedPerks?.Clear();
    }

    private void OnLevelUp(int level)
    {
        var perks = PickRandomPerks(3);
        // If no perks available, skip showing UI and don't pause the game
        if (perks == null || perks.Length == 0)
        {
            Debug.LogWarning("PerkSystem: no selectable perks to show on level up.");
            return;
        }

        // Protect UI reference
        if (perkSelectionUI != null)
        {
            perkSelectionUI.Show(perks, OnPerkSelected);
            GamePauseController.Pause();
        }
    }

    private void OnPerkSelected(PerkData perk)
    {
        if (perkContainer != null)
        {
            perkContainer.AddPerk(perk);
            GamePauseController.Resume();
        }
    }

    private PerkData[] PickRandomPerks(int count)
    {
        if (selectedPerks == null)
            return new PerkData[0];

        var selectable = selectedPerks.Where(p => p != null && p.isSelectable).ToList();

        if (selectable.Count == 0)
            return new PerkData[0];

        var result = new PerkData[count];
        for (int i = 0; i < count; i++)
        {
            // Allow duplicates if selectable.Count < count
            var choice = selectable[Random.Range(0, selectable.Count)];
            result[i] = choice;
        }

        return result;
    }
}