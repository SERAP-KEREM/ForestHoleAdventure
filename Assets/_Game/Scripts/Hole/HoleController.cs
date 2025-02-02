using UnityEngine;
using _Main.InputSystem;
using _Main._Level;

namespace _Main.Hole
{

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

       
    }
}