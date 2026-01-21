using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PerkButton : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private Button button;

    private PerkData perk;

    public void Setup(PerkData perk, Action<PerkData> onClick)
    {
        this.perk = perk;

        icon.sprite = perk.icon;
        title.text = perk.perkName;
        description.text = perk.description;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick(perk));
    }
}