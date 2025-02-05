using SerapKeremGameTools._Game._Singleton;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace SerapKeremGameTools._Game._InputSystem
{
    public class PlayerInput : MonoSingleton<PlayerInput>
    {
        [Header("Input Settings")]
        [SerializeField] private Joystick _joystick;
        [SerializeField] private bool _useKeyboardInput = true;
        [SerializeField] private float _keyboardInputSmoothing = 0.1f;

        public Vector2 MovementInput { get; private set; }
        public Vector3 MousePosition { get; private set; }
        public bool IsInputActive { get; private set; }

        // Movement Events
        public UnityEvent OnInputStarted = new UnityEvent();
        public UnityEvent OnInputEnded = new UnityEvent();
        public UnityEvent<Vector3> OnInputChanged = new UnityEvent<Vector3>();

        // Mouse Events
        public UnityEvent OnMouseDownEvent = new UnityEvent();
        public UnityEvent OnMouseHeldEvent = new UnityEvent();
        public UnityEvent OnMouseUpEvent = new UnityEvent();
        public UnityEvent<Vector3> OnMousePositionChanged = new UnityEvent<Vector3>();

        private Vector2 _keyboardInput;
        private Vector2 _currentKeyboardVelocity;
        private Camera _mainCamera;
        private bool _isInitialized;

        protected override void Awake()
        {
         
            base.Awake();
            Initialize();
        }
       
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        private void Initialize()
        {
            _mainCamera = Camera.main;
            FindJoystick();
            _isInitialized = true;
            EnableInput();
        }
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            StartCoroutine(InitializeAfterSceneLoad());
        }
        private IEnumerator InitializeAfterSceneLoad()
        {
            yield return new WaitForEndOfFrame();
            Initialize();
        }
        private void FindJoystick()
        {
            if (_joystick == null)
            {
                _joystick = FindObjectOfType<DynamicJoystick>();
               
            }
        }
        private void Update()
        {
            if (!_isInitialized) return;
            UpdateMouseInput();
            UpdateMovementInput();
        }

        private void UpdateMouseInput()
        {
            if (_mainCamera == null) return;

            // Update mouse position
            Vector3 previousMousePosition = MousePosition;
            MousePosition = Input.mousePosition;

            // Trigger position changed event if needed
            if (MousePosition != previousMousePosition)
            {
                OnMousePositionChanged?.Invoke(MousePosition);
            }

            // Handle mouse button events
            if (Input.GetMouseButtonDown(0))
            {
                OnMouseDownEvent?.Invoke();
            }
            if (Input.GetMouseButton(0))
            {
                OnMouseHeldEvent?.Invoke();
            }
            if (Input.GetMouseButtonUp(0))
            {
                OnMouseUpEvent?.Invoke();
            }
        }

        private void UpdateMovementInput()
        {
            Vector2 finalInput = Vector2.zero;
            bool isCurrentlyActive = false;

            // Joystick input
            if (_joystick != null && (_joystick.Direction != Vector2.zero))
            {
                finalInput = _joystick.Direction;
                isCurrentlyActive = true;
            }

            // Keyboard input
            if (_useKeyboardInput)
            {
                Vector2 targetKeyboardInput = new Vector2(
                    Input.GetAxisRaw("Horizontal"),
                    Input.GetAxisRaw("Vertical")
                );
                if (targetKeyboardInput.magnitude > 0.1f)
                {
                    _keyboardInput = Vector2.SmoothDamp(
                        _keyboardInput,
                        targetKeyboardInput,
                        ref _currentKeyboardVelocity,
                        _keyboardInputSmoothing
                    );

                    finalInput = _keyboardInput.normalized;
                    isCurrentlyActive = true;
                }
                else if (_keyboardInput.magnitude > 0)
                {
                    _keyboardInput = Vector2.SmoothDamp(
                        _keyboardInput,
                        Vector2.zero,
                        ref _currentKeyboardVelocity,
                        _keyboardInputSmoothing
                    );

                    if (_keyboardInput.magnitude > 0.1f)
                    {
                        finalInput = _keyboardInput.normalized;
                        isCurrentlyActive = true;
                    }
                }
            }

            MovementInput = finalInput;
            IsInputActive = isCurrentlyActive;

            if (MovementInput != Vector2.zero)
            {
                OnInputChanged?.Invoke(new Vector3(MovementInput.x, 0f, MovementInput.y));
            }
        }
       

        public void EnableInput()
        {
            _useKeyboardInput = true;
            
            _isInitialized = true;
        }

        public void DisableInput()
        {
            _useKeyboardInput = false;
           
            MovementInput = Vector2.zero;
            _keyboardInput = Vector2.zero;
            _currentKeyboardVelocity = Vector2.zero;
            _isInitialized = false;
        }
    }
}