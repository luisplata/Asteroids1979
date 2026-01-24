using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class BGController : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private RawImage bgImage;

        [SerializeField] private Rigidbody2D playerRigidbody;

        [Header("Parallax (Velocity Based)")]
        [Tooltip("Cuánto influye la velocidad del player en el desplazamiento del fondo")]
        [SerializeField]
        private float velocityStrength = 0.02f;

        [Tooltip("Invertir dirección para sensación de avance")] [SerializeField]
        private bool invertDirection = true;

        [Tooltip("Usar eje X")] [SerializeField]
        private bool useX = true;

        [Tooltip("Usar eje Y")] [SerializeField]
        private bool useY = true;

        [Header("UV Scaling")] [Tooltip("Escala del UV (zoom del fondo)")] [SerializeField]
        private Vector2 uvScale = Vector2.one;

        [Header("Smoothing")] [Tooltip("Suavizado del movimiento del fondo")] [SerializeField]
        private float smoothSpeed = 6f;

        [Header("Clamp (Optional)")] [Tooltip("Límite máximo del offset acumulado")] [SerializeField]
        private float maxOffset = 5f;

        private Vector2 _currentOffset;

        private void Awake()
        {
            if (bgImage != null)
                bgImage.uvRect = new Rect(Vector2.zero, uvScale);
        }

        private void Update()
        {
            if (bgImage == null || playerRigidbody == null)
                return;

            Vector2 velocity = playerRigidbody.linearVelocity;

            Vector2 deltaOffset = Vector2.zero;

            if (useX)
                deltaOffset.x = velocity.x;

            if (useY)
                deltaOffset.y = velocity.y;

            if (invertDirection)
                deltaOffset *= -1f;

            // 🔑 Integración de velocidad (esto es la clave)
            Vector2 targetOffset = _currentOffset +
                                   deltaOffset * (velocityStrength * Time.deltaTime);

            if (maxOffset > 0f)
                targetOffset = Vector2.ClampMagnitude(targetOffset, maxOffset);

            // Suavizado visual (opcional pero recomendable)
            _currentOffset = Vector2.Lerp(
                _currentOffset,
                targetOffset,
                Time.deltaTime * smoothSpeed
            );

            bgImage.uvRect = new Rect(_currentOffset, uvScale);
        }
    }
}