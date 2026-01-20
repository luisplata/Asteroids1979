using UnityEngine;

public abstract class PerkData : ScriptableObject
{
    [Header("Perk Info")]
    public string perkName;
    [TextArea] public string description;

    public abstract void Apply(Player player);
}