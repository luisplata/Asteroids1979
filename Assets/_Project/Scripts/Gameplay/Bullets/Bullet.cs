using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifeTime = 2f;

    private Rigidbody2D rb;
    private Vector2 lastVelocity;

    // Umbral para evitar jitter y cálculos innecesarios
    private const float velocityThreshold = 0.001f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lastVelocity = Vector2.zero;
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        Vector2 currentVelocity = rb.linearVelocity;

        // Si casi no se mueve, no rotamos
        if (currentVelocity.sqrMagnitude < velocityThreshold)
            return;

        // Solo recalcular si la velocidad cambió
        if ((currentVelocity - lastVelocity).sqrMagnitude > velocityThreshold)
        {
            UpdateRotation(currentVelocity);
            lastVelocity = currentVelocity;
        }
    }

    private void UpdateRotation(Vector2 velocity)
    {
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }
}