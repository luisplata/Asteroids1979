using UnityEngine;
using _Project.Scripts.Gameplay.Player;

public class PlayerBodyVisual : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D playerRigidbody2D;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private PlayerController playerController;

    [Header("Rotation VFX")]
    [SerializeField] private GameObject vaporEffectL;
    [SerializeField] private GameObject vaporEffectR;

    [Header("Propulsion VFX")]
    [SerializeField] private Transform propulsionEffect;
    [SerializeField] private Transform minPropulsionEffect;
    [SerializeField] private Transform maxPropulsionEffect;

    [Header("Tuning")]
    [Tooltip("Velocidad base al extender el propulsor (aceleración)")]
    [SerializeField] private float propulsionExtendSpeed = 10f;

    [Tooltip("Multiplicador visual del linearDamping para la recogida")]
    [SerializeField] private float dampingRetractMultiplier = 1.5f;

    private static readonly int IsInputHash = Animator.StringToHash("isInput");

    private void Update()
    {
        UpdateAnimator();
        UpdateRotationVfx();
        UpdatePropulsionVfx();
    }

    // =====================
    // Animator
    // =====================

    private void UpdateAnimator()
    {
        bool hasInput = playerController.ReadInput().sqrMagnitude > 0.01f;
        playerAnimator.SetBool(IsInputHash, hasInput);
    }

    // =====================
    // Rotation VFX
    // =====================

    private void UpdateRotationVfx()
    {
        if (playerController.IsRotatingClockwise())
        {
            vaporEffectL.SetActive(true);
            vaporEffectR.SetActive(false);
        }
        else if (playerController.IsRotatingCounterClockwise())
        {
            vaporEffectL.SetActive(false);
            vaporEffectR.SetActive(true);
        }
        else
        {
            vaporEffectL.SetActive(false);
            vaporEffectR.SetActive(false);
        }
    }

    // =====================
    // Propulsion VFX
    // =====================

    private void UpdatePropulsionVfx()
    {
        Vector3 targetPosition;
        float lerpSpeed;

        if (playerController.IsThrusting)
        {
            // 🔥 Acelerando → respuesta rápida
            targetPosition = maxPropulsionEffect.localPosition;
            lerpSpeed = propulsionExtendSpeed;
        }
        else
        {
            // 🧊 Desacelerando → ligado al damping de la nave
            targetPosition = minPropulsionEffect.localPosition;

            float damping = Mathf.Max(0.01f, playerRigidbody2D.linearDamping);
            lerpSpeed = damping * dampingRetractMultiplier;
        }

        propulsionEffect.localPosition = Vector3.Lerp(
            propulsionEffect.localPosition,
            targetPosition,
            Time.deltaTime * lerpSpeed
        );
    }
}