using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    [Header("Offense")]
    public float damage = 1f;
    public float fireRate = 2f;

    [Header("Survivability")]
    public float maxHealth = 5f;
    [HideInInspector] public float currentHealth;

    public void Init()
    {
        currentHealth = maxHealth;
    }
}