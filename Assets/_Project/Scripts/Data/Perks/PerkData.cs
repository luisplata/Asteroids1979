using UnityEngine;

public abstract class PerkData : ScriptableObject
{
    public string perkName;
    public Sprite icon;
    public string description;
    public bool isSelectable = true;

    public abstract void Apply(Player target);
}