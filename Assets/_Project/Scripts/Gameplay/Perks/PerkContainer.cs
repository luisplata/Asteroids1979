using System.Collections.Generic;
using UnityEngine;

public class PerkContainer : MonoBehaviour
{
    private Player player;
    private readonly List<PerkData> activePerks = new();

    void Awake()
    {
        player = GetComponent<Player>();
    }

    public void AddPerk(PerkData perk)
    {
        perk.Apply(player);
        activePerks.Add(perk);

        Debug.Log($"Perk aplicado: {perk.perkName}");
    }
}