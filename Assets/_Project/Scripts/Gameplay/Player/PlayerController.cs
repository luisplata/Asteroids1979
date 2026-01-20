using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float orbitRadius = 4f;

    private float angle;

    void Update()
    {
        angle += moveSpeed * Time.deltaTime;

        float x = Mathf.Cos(angle) * orbitRadius;
        float y = Mathf.Sin(angle) * orbitRadius;

        // transform.position = new Vector3(x, y, 0f);
    }
}