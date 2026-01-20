using UnityEngine;

namespace _Project.Scripts.Gameplay.Player
{
    public class PlayerVisual : MonoBehaviour
    {
        [SerializeField] private AutoShooter autoShooter;

        private void FixedUpdate()
        {
            //rotar hacia la direccion del Direction del autoshooter
            Vector2 direction = autoShooter.Direction;
            if (direction != Vector2.zero)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.Euler(0f, 0f, angle - 90f);
                transform.rotation = rotation;
            }
        }
    }
}