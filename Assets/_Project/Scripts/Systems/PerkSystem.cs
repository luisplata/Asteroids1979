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
        ScoreSystem.Instance.OnLevelUp += OnLevelUp;
        selectedPerks = new List<PerkData>();
        foreach (var availablePerk in availablePerks)
        {
            selectedPerks.Add(Instantiate(availablePerk));
        }
    }

    void OnDisable()
    {
        ScoreSystem.Instance.OnLevelUp -= OnLevelUp;
        selectedPerks.Clear();
    }

    private void OnLevelUp(int level)
    {
        var perks = PickRandomPerks(3);
        perkSelectionUI.Show(perks, OnPerkSelected);
        GamePauseController.Pause();
    }

    private void OnPerkSelected(PerkData perk)
    {
        perkContainer.AddPerk(perk);
        GamePauseController.Resume();
    }

    private PerkData[] PickRandomPerks(int count)
    {
        var result = new PerkData[count];
        for (int i = 0; i < count; i++)
        {
            List<PerkData> list = selectedPerks.Where(p => p.isSelectable).ToList();

            result[i] = list[Random.Range(0, list.Count)];
        }

        return result;
    }
}