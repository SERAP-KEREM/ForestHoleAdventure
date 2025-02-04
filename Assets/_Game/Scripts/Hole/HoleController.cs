using UnityEngine;
using _Main._Level;
using _Main._InputSystem;

namespace _Main._Hole
{
    /// <summary>
    /// Controller responsible for handling the hole's movement and input.
    /// </summary>
    public class HoleController : MonoBehaviour
    {
        #region Serialized Fields

        [Header("Level Settings")]
        [SerializeField, Tooltip("Reference to the Level Manager.")]
        private LevelManager _levelManager;

        [Header("Movement Settings")]
        [SerializeField, Tooltip("Movement speed of the hole.")]
        private float _moveSpeed = 5f;

        [Header("Input Settings")]
        [SerializeField, Tooltip("Floating joystick for mobile input.")]
        private FloatingJoystick _floatingJoystick;

        [SerializeField, Tooltip("Keyboard input handler for desktop control.")]
        private KeyboardInput _keyboardInput;

        #endregion

        private bool _isControlEnabled = true;

        private void Start()
        {
            // Find input system objects in the scene if not already assigned
            if (_floatingJoystick == null)
            {
                _floatingJoystick = FindObjectOfType<FloatingJoystick>();
            }

            if (_keyboardInput == null)
            {
                _keyboardInput = FindObjectOfType<KeyboardInput>();
            }
        }

        private void Update()
        {
            // Handle movement when control is enabled
            if (_isControlEnabled)
            {
                HandleMovement();
            }
        }

        /// <summary>
        /// Handles the movement input and updates the hole's position.
        /// </summary>
        private void HandleMovement()
        {
            // Get input from both the joystick and keyboard
            float horizontal = _floatingJoystick.Horizontal + _keyboardInput.Horizontal;
            float vertical = _floatingJoystick.Vertical + _keyboardInput.Vertical;

            // Calculate the movement direction and normalize it
            Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized;

            // Move the hole based on the calculated direction and speed
            transform.Translate(movement * _moveSpeed * Time.deltaTime, Space.World);
        }

        /// <summary>
        /// Enables the control for the hole (e.g., for player input).
        /// </summary>
        public void EnableControl()
        {
            _isControlEnabled = true;
            if (_floatingJoystick != null)
            {
                _floatingJoystick.Enable();
            }
        }

        /// <summary>
        /// Disables the control for the hole (e.g., to stop player input).
        /// </summary>
        public void DisableControl()
        {
            _isControlEnabled = false;
            if (_floatingJoystick != null)
            {
                _floatingJoystick.Disable();
            }
        }
    }
}
