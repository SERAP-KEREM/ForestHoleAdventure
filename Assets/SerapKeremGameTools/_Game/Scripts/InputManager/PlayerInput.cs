using SerapKeremGameTools._Game._Singleton;
using UnityEngine;
using UnityEngine.Events;

namespace SerapKeremGameTools._Game._InputSystem
{
    /// <summary>
    /// Handles player input, including mouse position tracking and mouse events.
    /// </summary>
    public class PlayerInput : MonoSingleton<PlayerInput>
    {
        /// <summary>
        /// Current mouse position in world coordinates.
        /// </summary>
        [Tooltip("Current mouse position in world coordinates.")]
        public Vector3 MousePosition { get; private set; }

        /// <summary>
        /// Event invoked when the left mouse button is pressed down.
        /// </summary>
        [Tooltip("Event triggered when the left mouse button is pressed down.")]
        public UnityEvent OnMouseDownEvent = new UnityEvent();

        /// <summary>
        /// Event invoked while the left mouse button is held down.
        /// </summary>
        [Tooltip("Event triggered while the left mouse button is held down.")]
        public UnityEvent OnMouseHeldEvent = new UnityEvent();

        /// <summary>
        /// Event invoked when the left mouse button is released.
        /// </summary>
        [Tooltip("Event triggered when the left mouse button is released.")]
        public UnityEvent OnMouseUpEvent = new UnityEvent();

        /// <summary>
        /// Event invoked when mouse position changes.
        /// </summary>
        [Tooltip("Event triggered when mouse position changes.")]
        public UnityEvent<Vector3> OnMousePositionInput = new UnityEvent<Vector3>();

        /// <summary>
        /// Movement input (Vector2) based on horizontal and vertical axes.
        /// </summary>
        [Tooltip("Current movement input (WASD or arrow keys).")]
        public Vector2 MovementInput { get; private set; }

        [Tooltip("Reference to the main camera in the scene.")]
        private Camera mainCamera;

        /// <summary>
        /// Initializes the PlayerInput singleton and assigns the main camera.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
#if UNITY_EDITOR
                Debug.LogError("No Main Camera found in the scene!");
#endif
            }
        }
        /// <summary>
        /// Updates input states, including mouse position and movement input.
        /// </summary>
        private void Update()
        {
            // Update mouse position
            if (mainCamera != null)
            {
                Vector3 previousMousePosition = MousePosition; // Store previous position
                MousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.nearClipPlane));

                // Trigger event if mouse position has changed
                if (MousePosition != previousMousePosition)
                {
                    OnMousePositionInput.Invoke(MousePosition);
                }
            }

            // Update movement input
            MovementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            // Mouse input events
            if (Input.GetMouseButtonDown(0)) // Left mouse button pressed
            {
                OnMouseDownEvent.Invoke();
            }

            if (Input.GetMouseButton(0)) // Left mouse button held
            {
                OnMouseHeldEvent.Invoke();
            }

            if (Input.GetMouseButtonUp(0)) // Left mouse button released
            {
                OnMouseUpEvent.Invoke();
            }
        }

        /// <summary>
        /// Returns the movement input for the player (WASD or arrow keys).
        /// </summary>
        public Vector2 GetMovementInput()
        {
            float moveX = Input.GetAxisRaw("Horizontal");  
            float moveY = Input.GetAxisRaw("Vertical");   
            return new Vector2(moveX, moveY);
        }
    }
}
