using System;
using UnityEngine;

public class PerkSelectionUI : MonoBehaviour
{
    [SerializeField] private PerkButton[] perkButtons;
    [SerializeField] private GameObject root;

    private Action<PerkData> onSelected;

    private void Start()
    {
        root.SetActive(false);
    }

    public void Show(PerkData[] perks, Action<PerkData> onSelected)
    {
        this.onSelected = onSelected;
        root.SetActive(true);

        for (int i = 0; i < perkButtons.Length; i++)
        {
            perkButtons[i].Setup(perks[i], Select);
        }
    }

    private void Select(PerkData perk)
    {
        root.SetActive(false);
        onSelected?.Invoke(perk);
    }
}