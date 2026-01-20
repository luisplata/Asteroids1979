using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStats stats;

    void Awake()
    {
        stats.Init();
    }
}