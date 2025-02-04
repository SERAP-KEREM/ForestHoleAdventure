using DG.Tweening;
using UnityEngine;
using TriInspector;

namespace _Main._Animals
{
    /// <summary>
    /// Controls the behavior and movement of animals, including idle, walking, running, and obstacle avoidance.
    /// </summary>
    public class AnimalController : MonoBehaviour
    {
        #region Movement Settings
        [Header("Movement Settings")]
        [SerializeField, Tooltip("Normal walking speed of the animal.")]
        private float _moveSpeed = 2f;

        [SerializeField, Tooltip("Running speed of the animal.")]
        private float _runSpeed = 5f;

        [SerializeField, Tooltip("Rotation speed of the animal.")]
        private float _rotationSpeed = 120f;

        [SerializeField, Tooltip("Maximum movement radius around the starting position.")]
        private float _moveRadius = 30f;

        [SerializeField, Tooltip("Radius for detecting the hole proximity.")]
        private float _holeDetectionRadius = 10f;

        [SerializeField, Tooltip("Distance for obstacle detection.")]
        private float _obstacleCheckDistance = 2f;
        #endregion

        #region Idle Settings
        [Header("Idle Settings")]
        [SerializeField, Tooltip("Minimum idle duration.")]
        private float _idleDurationMin = 2f;

        [SerializeField, Tooltip("Maximum idle duration.")]
        private float _idleDurationMax = 5f;
        #endregion

        #region Collection Settings
        [Header("Collection Settings")]
        [SerializeField, Tooltip("Duration for the animal to be collected.")]
        private float _collectDuration = 1f;
        #endregion

        #region Private Variables
        private Animator _animator;
        private Transform _holeTransform;
        private Vector3 _targetPosition;
        private bool _isIdle;
        private float _idleTimer;
        private float _currentSpeed;
        private Vector3 _startPosition;
        private bool _isCollected;
        #endregion

        #region Animation Hashes
        private static readonly int IsIdleHash = Animator.StringToHash("isIdle");
        private static readonly int IsWalkingHash = Animator.StringToHash("isWalking");
        private static readonly int IsRunningHash = Animator.StringToHash("isRunning");
        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        #endregion

        #region Unity Lifecycle Methods
        private void Start()
        {
            InitializeComponents();
            _startPosition = transform.position;
            SetNewTarget();
            _currentSpeed = _moveSpeed;
        }

        private void Update()
        {
            if (_isIdle || _isCollected)
            {
                HandleIdleState();
                return;
            }

            CheckHoleProximity();
            MoveToTarget();
        }

        private void OnDrawGizmosSelected()
        {
            DrawDebugGizmos();
        }
        #endregion

        #region Initialization
        private void InitializeComponents()
        {
            _animator = GetComponentInChildren<Animator>();

            GameObject holeObject = GameObject.FindGameObjectWithTag("Player");
            if (holeObject != null)
            {
                _holeTransform = holeObject.transform;
            }
            else
            {
                Debug.LogError("Hole not found. Ensure the hole object has the 'Player' tag.");
            }
        }
        #endregion

        #region Movement Logic
        private void MoveToTarget()
        {
            if (Vector3.Distance(transform.position, _targetPosition) < 0.5f)
            {
                StartIdleState();
                return;
            }

            if (CheckForObstacle())
            {
                SetNewTarget();
                return;
            }

            RotateTowardsTarget();
            transform.position += transform.forward * _currentSpeed * Time.deltaTime;

            if (Vector3.Distance(_startPosition, transform.position) > _moveRadius)
            {
                SetNewTarget();
            }

            UpdateAnimationState();
        }

        private void RotateTowardsTarget()
        {
            Vector3 direction = (_targetPosition - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                _rotationSpeed * Time.deltaTime
            );
        }

        private bool CheckForObstacle()
        {
            if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out RaycastHit hit, _obstacleCheckDistance))
            {
                if (!hit.collider.CompareTag("Player") && !hit.collider.CompareTag("Animal"))
                {
                    Debug.Log($"Obstacle detected: {hit.collider.name}");
                    return true;
                }
            }
            return false;
        }

        private void CheckHoleProximity()
        {
            if (_holeTransform == null) return;

            float distanceToHole = Vector3.Distance(transform.position, _holeTransform.position);

            if (distanceToHole <= _holeDetectionRadius)
            {
                _currentSpeed = _runSpeed;
                Vector3 awayFromHole = transform.position - _holeTransform.position;
                _targetPosition = transform.position + awayFromHole.normalized * _moveRadius;
                _targetPosition.y = transform.position.y;
            }
            else
            {
                _currentSpeed = _moveSpeed;
            }
        }
        #endregion

        #region Idle Logic
        private void HandleIdleState()
        {
            _idleTimer -= Time.deltaTime;
            if (_idleTimer <= 0)
            {
                _isIdle = false;
                SetNewTarget();
                UpdateAnimationState();
            }
        }

        private void StartIdleState()
        {
            _isIdle = true;
            _idleTimer = Random.Range(_idleDurationMin, _idleDurationMax);
            UpdateAnimationState();
        }
        #endregion

        #region Utility Methods
        private void SetNewTarget()
        {
            Vector2 randomCircle = Random.insideUnitCircle * _moveRadius;
            _targetPosition = _startPosition + new Vector3(randomCircle.x, 0, randomCircle.y);

            Vector3 direction = (_targetPosition - transform.position).normalized;
            transform.DORotateQuaternion(Quaternion.LookRotation(direction), 0.5f);
        }

        private void UpdateAnimationState()
        {
            if (_animator == null) return;

            _animator.SetBool(IsIdleHash, _isIdle);
            _animator.SetBool(IsWalkingHash, !_isIdle && _currentSpeed == _moveSpeed);
            _animator.SetBool(IsRunningHash, !_isIdle && _currentSpeed == _runSpeed);
            _animator.SetFloat(SpeedHash, _currentSpeed);
        }

        private void DrawDebugGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(Application.isPlaying ? _startPosition : transform.position, _moveRadius);

            if (Application.isPlaying)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(_targetPosition, 0.5f);

                Gizmos.color = Color.red;
                Gizmos.DrawRay(transform.position + Vector3.up, transform.forward * _obstacleCheckDistance);
            }
        }
        #endregion
    }
}
