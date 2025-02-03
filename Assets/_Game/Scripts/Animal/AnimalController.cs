using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace _Main._Animals
{
    public class AnimalController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float runSpeed = 5f;
        [SerializeField] private float rotationSpeed = 120f;
        [SerializeField] private float moveRadius = 30f;
        [SerializeField] private float holeDetectionRadius = 10f;
        [SerializeField] private float obstacleCheckDistance = 2f;

        [Header("Idle Settings")]
        [SerializeField] private float idleDurationMin = 2f;
        [SerializeField] private float idleDurationMax = 5f;

        private Animator _animator;
        private Transform _holeTransform;
        private Vector3 _targetPosition;
        private bool _isIdle;
        private float _idleTimer;
        private float _currentSpeed;
        private Vector3 _startPosition; // Spawn pozisyonu

        private static readonly int IsIdle = Animator.StringToHash("isIdle");
        private static readonly int IsWalking = Animator.StringToHash("isWalking");
        private static readonly int IsRunning = Animator.StringToHash("isRunning");
        private static readonly int Speed = Animator.StringToHash("Speed");

        private void Start()
        {
            InitializeComponents();
            _startPosition = transform.position;
            SetNewTarget();
            _currentSpeed = moveSpeed;
        }

        private void InitializeComponents()
        {
            _animator = GetComponentInChildren<Animator>();
            GameObject holeObject = GameObject.FindGameObjectWithTag("Player");
            if (holeObject != null)
            {
                _holeTransform = holeObject.transform;
            }
        }

        private void Update()
        {
            if (_isIdle)
            {
                HandleIdleState();
                return;
            }

            CheckHoleProximity();
            MoveToTarget();
        }

        private void MoveToTarget()
        {
            if (Vector3.Distance(transform.position, _targetPosition) < 0.5f)
            {
                StartIdleState();
                return;
            }

            // Engel kontrolü
            if (CheckForObstacle())
            {
                SetNewTarget();
                return;
            }

            // Hedefe doğru dön
            Vector3 direction = (_targetPosition - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            // İleri hareket
            transform.position += transform.forward * _currentSpeed * Time.deltaTime;

            // Sınırları kontrol et
            if (Vector3.Distance(_startPosition, transform.position) > moveRadius)
            {
                SetNewTarget();
            }

            UpdateAnimationState();
        }

        private bool CheckForObstacle()
        {
            // İleri doğru engel kontrolü
            if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out RaycastHit hit, obstacleCheckDistance))
            {
                // Hole veya başka bir hayvan değilse engel olarak kabul et
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
            if (_holeTransform != null)
            {
                float distanceToHole = Vector3.Distance(transform.position, _holeTransform.position);

                if (distanceToHole <= holeDetectionRadius)
                {
                    // Delikten kaç
                    _currentSpeed = runSpeed;
                    Vector3 awayFromHole = transform.position - _holeTransform.position;
                    _targetPosition = transform.position + awayFromHole.normalized * moveRadius;
                    _targetPosition.y = transform.position.y;
                }
                else
                {
                    _currentSpeed = moveSpeed;
                }
            }
        }

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
            _idleTimer = Random.Range(idleDurationMin, idleDurationMax);
            UpdateAnimationState();
        }

        private void SetNewTarget()
        {
            Vector2 randomCircle = Random.insideUnitCircle * moveRadius;
            _targetPosition = _startPosition + new Vector3(randomCircle.x, 0, randomCircle.y);

            // Smooth dönüş animasyonu
            Vector3 direction = (_targetPosition - transform.position).normalized;
            transform.DORotateQuaternion(Quaternion.LookRotation(direction), 0.5f);
        }

        private void UpdateAnimationState()
        {
            if (_animator != null)
            {
                _animator.SetBool(IsIdle, _isIdle);
                _animator.SetBool(IsWalking, !_isIdle && _currentSpeed == moveSpeed);
                _animator.SetBool(IsRunning, !_isIdle && _currentSpeed == runSpeed);
                _animator.SetFloat(Speed, _currentSpeed);
            }
        }

        private void OnDrawGizmosSelected()
        {
            // Hareket alanını görselleştir
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(Application.isPlaying ? _startPosition : transform.position, moveRadius);

            // Hedef noktayı görselleştir
            if (Application.isPlaying)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(_targetPosition, 0.5f);

                // Engel kontrol mesafesini görselleştir
                Gizmos.color = Color.red;
                Gizmos.DrawRay(transform.position + Vector3.up, transform.forward * obstacleCheckDistance);
            }
        }
    }
}
