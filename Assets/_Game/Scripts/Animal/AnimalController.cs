using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

namespace _Main._Animals
{
    public class AnimalController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float _walkSpeed = 2f;
        [SerializeField] private float _runSpeed = 5f;
        [SerializeField] private float _moveRadius = 10f;
        [SerializeField] private float _minDistanceToObstacles = 2f;

        [Header("Timing Settings")]
        [SerializeField] private float _idleTimeMin = 2f;
        [SerializeField] private float _idleTimeMax = 5f;

        [Header("Hole Detection")]
        [SerializeField] private float _holeDetectionRadius = 10f;
        [SerializeField] private float _runAwayDistance = 15f;

        private NavMeshAgent _agent;
        private Animator _animator;
        private Transform _holeTransform;
        private Vector3 _startPosition;
        private bool _isIdle = true;
        private float _idleTimer;

        private static readonly int IsIdle = Animator.StringToHash("isIdle");
        private static readonly int IsWalking = Animator.StringToHash("isWalking");
        private static readonly int IsRunning = Animator.StringToHash("isRunning");

        private void Start()
        {
            InitializeComponents();
            _startPosition = transform.position;
            StartIdleState();
        }

        private void InitializeComponents()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();
            _holeTransform = GameObject.FindGameObjectWithTag("Player")?.transform;

            if (_agent != null)
            {
                _agent.speed = _walkSpeed;
                _agent.stoppingDistance = 0.5f;
                _agent.autoBraking = true;
                _agent.acceleration = 8f;
                _agent.angularSpeed = 120f;
            }
        }

        private void Update()
        {
            if (_agent == null || _holeTransform == null) return;

            CheckHoleProximity();

            if (_isIdle)
            {
                UpdateIdleState();
            }
            else if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                StartIdleState();
            }

            UpdateAnimation();
        }

        private void CheckHoleProximity()
        {
            float distanceToHole = Vector3.Distance(transform.position, _holeTransform.position);

            if (distanceToHole <= _holeDetectionRadius)
            {
                _agent.speed = _runSpeed;
                Vector3 directionFromHole = (transform.position - _holeTransform.position).normalized;
                Vector3 runToPosition = transform.position + directionFromHole * _runAwayDistance;

                if (NavMesh.SamplePosition(runToPosition, out NavMeshHit hit, _runAwayDistance, NavMesh.AllAreas))
                {
                    _agent.SetDestination(hit.position);
                    _isIdle = false;
                }
            }
            else
            {
                _agent.speed = _walkSpeed;
            }
        }

        private void UpdateIdleState()
        {
            _idleTimer -= Time.deltaTime;

            if (_idleTimer <= 0)
            {
                MoveToRandomPosition();
            }
        }

        private void StartIdleState()
        {
            _isIdle = true;
            _idleTimer = Random.Range(_idleTimeMin, _idleTimeMax);
            _agent.ResetPath();
        }

        private void MoveToRandomPosition()
        {
            for (int i = 0; i < 5; i++) // 5 kez deneme yap
            {
                Vector3 randomDirection = Random.insideUnitSphere * _moveRadius;
                randomDirection.y = 0;
                Vector3 targetPosition = _startPosition + randomDirection;

                if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, _moveRadius, NavMesh.AllAreas))
                {
                    // Hedef pozisyonun etrafında engel kontrolü
                    if (!Physics.CheckSphere(hit.position, _minDistanceToObstacles))
                    {
                        _agent.SetDestination(hit.position);
                        _isIdle = false;
                        return;
                    }
                }
            }
        }

        private void UpdateAnimation()
        {
            if (_animator == null) return;

            float currentSpeed = _agent.velocity.magnitude;
            bool isMoving = currentSpeed > 0.1f;
            bool isRunning = currentSpeed > _walkSpeed + 0.1f;

            _animator.SetBool(IsIdle, !isMoving);
            _animator.SetBool(IsWalking, isMoving && !isRunning);
            _animator.SetBool(IsRunning, isRunning);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _holeDetectionRadius);

            if (Application.isPlaying && _startPosition != Vector3.zero)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(_startPosition, _moveRadius);
            }
        }
    }
}