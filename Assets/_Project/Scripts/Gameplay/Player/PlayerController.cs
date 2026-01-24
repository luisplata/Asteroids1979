using UnityEngine;

namespace _Project.Scripts.Gameplay.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float thrust = 1f;
        [SerializeField] private float maxSpeed = 2f;
        [SerializeField] private float linearDamping = 1f;

        [Header("Rotation")]
        [SerializeField] private float turnSpeed = 180f; // degrees per second

        [Header("Input")]
        [SerializeField] private Joystick joystick;

        private Rigidbody2D _rb;

        // Rotation state
        private float _targetAngle;
        private int _rotationSign; // -1 = clockwise, +1 = counter-clockwise, 0 = none

        private const float InputDeadZone = 0.1f;
        private const float AngleTolerance = 2f;

        // ===== Public read-only state =====
        public float MaxSpeed => maxSpeed;
        public bool IsThrusting { get; private set; }

        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();

            // Nave espacial "flotante"
            _rb.gravityScale = 0f;
            _rb.linearDamping = linearDamping;
            _rb.angularDamping = 0f;

            // Inicia mirando hacia arriba (Y+)
            _rb.rotation = 0f;
            _targetAngle = 0f;
        }

        void FixedUpdate()
        {
            Vector2 input = ReadInput();
            bool hasInput = input.sqrMagnitude > InputDeadZone * InputDeadZone;

            // Reset cada frame (muy importante para visuales)
            IsThrusting = false;

            if (hasInput)
            {
                input.Normalize();

                // Input define intención de rotación
                _targetAngle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg - 90f;

                RotateTowardsTarget();
                ApplyThrustWhenAligned();
            }
            else
            {
                // Sin input → no rotación activa
                _rotationSign = 0;
            }

            ClampSpeed();
        }

        // =====================
        // Rotation
        // =====================

        private void RotateTowardsTarget()
        {
            float angleDelta = Mathf.DeltaAngle(_rb.rotation, _targetAngle);

            // Dirección de rotación (para VFX / animaciones)
            if (Mathf.Abs(angleDelta) < AngleTolerance)
                _rotationSign = 0;
            else
                _rotationSign = angleDelta > 0 ? 1 : -1;

            float newAngle = Mathf.MoveTowardsAngle(
                _rb.rotation,
                _targetAngle,
                turnSpeed * Time.fixedDeltaTime
            );

            _rb.MoveRotation(newAngle);
        }

        // =====================
        // Thrust
        // =====================

        private void ApplyThrustWhenAligned()
        {
            float angleDelta = Mathf.Abs(Mathf.DeltaAngle(_rb.rotation, _targetAngle));

            // Solo empuja cuando está alineada
            if (angleDelta <= AngleTolerance)
            {
                Vector2 forward = transform.up;
                _rb.AddForce(forward * thrust, ForceMode2D.Force);
                IsThrusting = true;
            }
        }

        // =====================
        // Speed
        // =====================

        private void ClampSpeed()
        {
            if (_rb.linearVelocity.magnitude > maxSpeed)
            {
                _rb.linearVelocity = _rb.linearVelocity.normalized * maxSpeed;
            }
        }

        // =====================
        // Input
        // =====================

        public Vector2 ReadInput()
        {
            if (joystick != null)
            {
                return new Vector2(joystick.Horizontal, joystick.Vertical);
            }

            return new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            );
        }

        // =====================
        // PUBLIC API (VFX / Animator)
        // =====================

        public bool IsRotatingClockwise()
        {
            return _rotationSign < 0;
        }

        public bool IsRotatingCounterClockwise()
        {
            return _rotationSign > 0;
        }
    }
}