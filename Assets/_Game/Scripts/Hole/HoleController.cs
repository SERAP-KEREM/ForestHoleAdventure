using UnityEngine;
using _Main.InputSystem;

namespace _Main.Hole
{
    using _Main._Level;
    using _Main._Objects;
    using UnityEngine;

    public class HoleController : MonoBehaviour
    {
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private FloatingJoystick floatingJoystick;
        [SerializeField] private KeyboardInput keyboardInput;

        private void Start()
        {
            // Sahnedeki FloatingJoystick ve KeyboardInput bileşenlerini bul
            floatingJoystick = FindObjectOfType<FloatingJoystick>();
            keyboardInput = FindObjectOfType<KeyboardInput>();
        }
        private void Update()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            // Joystick ve klavye inputlarını birleştir
            float horizontal = floatingJoystick.Horizontal + keyboardInput.Horizontal;
            float vertical = floatingJoystick.Vertical + keyboardInput.Vertical;

            Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized;
            transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
        }

        private void OnTriggerEnter(Collider other)
        {
            CollectibleItem collectible = other.GetComponent<CollectibleItem>();

            if (collectible != null && !collectible.IsCollected)
            {
                // Deliğin boyutu nesnenin boyutundan büyükse yut
                if (transform.localScale.x > collectible.Size)
                {
                    levelManager.AddScore(collectible.Score);
                    collectible.Collect();
                }
            }
        }
    }
}