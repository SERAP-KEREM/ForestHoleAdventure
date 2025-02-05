using UnityEngine;
using SerapKeremGameTools._Game._InputSystem;
using TriInspector;
using _Main._Level;
using SerapKeremGameTools._Game._Singleton;

namespace _Main._Hole
{
    [DeclareFoldoutGroup("Movement Settings", Title = "Movement Settings")]
    public class HoleController : MonoSingleton<HoleController>
    {
        [Group("Movement Settings")]
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _movementSmoothing = 0.1f;

        private bool _isControlEnabled = true;
        private Vector3 _currentVelocity;
        private Vector3 _targetPosition;

        protected override void Awake()
        {
            base.Awake();
        }
        private void Start()
        {
            _targetPosition = transform.position;

            if (PlayerInput.Instance != null)
            {
                PlayerInput.Instance.OnInputStarted.AddListener(OnInputStarted);
                PlayerInput.Instance.OnInputEnded.AddListener(OnInputEnded);
            }
        }

        private void OnDestroy()
        {
            if (PlayerInput.Instance != null)
            {
                PlayerInput.Instance.OnInputStarted.RemoveListener(OnInputStarted);
                PlayerInput.Instance.OnInputEnded.RemoveListener(OnInputEnded);
            }
        }

        private void Update()
        {
            if (!_isControlEnabled || PlayerInput.Instance == null) return;

            Vector2 input = PlayerInput.Instance.MovementInput;
            if (input != Vector2.zero)
            {
                Vector3 movement = new Vector3(input.x, 0f, input.y);
                _targetPosition += movement * _moveSpeed * Time.deltaTime;
            }

            // Smooth movement
            transform.position = Vector3.SmoothDamp(
                transform.position,
                _targetPosition,
                ref _currentVelocity,
                _movementSmoothing
            );
        }

        private void OnInputStarted()
        {
            if (_isControlEnabled)
            {
                _targetPosition = transform.position;
            }
        }

        private void OnInputEnded()
        {
            _currentVelocity = Vector3.zero;
        }

        public void EnableControl()
        {
            _isControlEnabled = true;
            if (PlayerInput.Instance != null)
            {
                PlayerInput.Instance.EnableInput();
            }
        }

        public void DisableControl()
        {
            _isControlEnabled = false;
            if (PlayerInput.Instance != null)
            {
                PlayerInput.Instance.DisableInput();
            }
            _currentVelocity = Vector3.zero;
        }
    }
}