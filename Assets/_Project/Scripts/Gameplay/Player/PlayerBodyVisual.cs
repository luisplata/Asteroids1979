using System;
using _Project.Scripts.Bootstrap;
using UnityEngine;
using _Project.Scripts.Gameplay.Player;

public class PlayerBodyVisual : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private Rigidbody2D playerRigidbody2D;

    [SerializeField] private Animator playerAnimator;
    [SerializeField] private PlayerController playerController;

    [Header("Rotation VFX")] [SerializeField]
    private GameObject vaporEffectL;

    [SerializeField] private GameObject vaporEffectR;

    [Header("Propulsion VFX")] [SerializeField]
    private Transform propulsionEffect;

    [SerializeField] private Transform minPropulsionEffect;
    [SerializeField] private Transform maxPropulsionEffect;

    [Header("Tuning")] [SerializeField] private float propulsionExtendSpeed = 10f;
    [SerializeField] private float dampingRetractMultiplier = 1.5f;

    private static readonly int IsInputHash = Animator.StringToHash("isInput");

    // ===== Cached state =====
    private bool _wasThrusting;
    private int _lastRotationSign; // -1, 0, +1

    private bool _isGameplay;

    private void Start()
    {
        GameBootstrap.Instance.GameState.OnGameStarted += GameStateOnOnGameStarted;
        GameBootstrap.Instance.GameState.OnGameOver += GameStateOnOnGameOver;
    }

    private void GameStateOnOnGameOver()
    {
        _isGameplay = false;
    }

    private void GameStateOnOnGameStarted()
    {
        _isGameplay = true;
    }

    private void OnDisable()
    {
        GameBootstrap.Instance.GameState.OnGameStarted -= GameStateOnOnGameStarted;
        GameBootstrap.Instance.GameState.OnGameOver -= GameStateOnOnGameOver;
    }

    private void Update()
    {
        if (!_isGameplay) return;
        UpdateAnimator();
        UpdateRotationVfx();
        UpdatePropulsionVfx();
        UpdateAudio();
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
            targetPosition = maxPropulsionEffect.localPosition;
            lerpSpeed = propulsionExtendSpeed;
        }
        else
        {
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

    // =====================
    // Audio (STATE → SOUND)
    // =====================

    private void UpdateAudio()
    {
        _wasThrusting = playerController.IsThrusting;

        // 🔄 Rotation sound (cuando empieza a girar)
        int currentRotationSign = 0;

        if (playerController.IsRotatingClockwise())
            currentRotationSign = -1;
        else if (playerController.IsRotatingCounterClockwise())
            currentRotationSign = 1;

        _lastRotationSign = currentRotationSign;
    }
}