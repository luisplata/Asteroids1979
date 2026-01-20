using UnityEngine;

public class TestPerkApplier : MonoBehaviour
{
    [SerializeField] private PerkData perkToApply;
    [SerializeField] private float delay = 3f;

    void Start()
    {
        Invoke(nameof(Apply), delay);
    }

    void Apply()
    {
        GetComponent<PerkContainer>().AddPerk(perkToApply);
    }
}