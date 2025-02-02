using UnityEngine;
using UnityEngine.AI;

namespace _Main._Animals
{
    public class AnimalController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float idleDurationMin = 2f;
        [SerializeField] private float idleDurationMax = 5f;
        [SerializeField] private float holeDetectionRadius = 10f;

        private NavMeshAgent _agent;
        private Animator _animator;
        private Transform _holeTransform;

        private bool _isIdle = false;
        private float _idleTimer = 0f;
        private float _idleDuration = 0f;

        private static readonly int IsIdle = Animator.StringToHash("isIdle");
        private static readonly int IsWalking = Animator.StringToHash("isWalking");
        private static readonly int IsRunning = Animator.StringToHash("isRunning");
        private static readonly int Speed = Animator.StringToHash("Speed");

        private void Start()
        {
            InitializeComponents();
            SetIdleState();
        }

        private void Update()
        {
            if (_isIdle)
            {
                HandleIdleState();
            }
            else
            {
                HandleMovement();
            }

            if (_holeTransform != null)
            {
                CheckHoleProximity();
            }
        }

        private void LateUpdate()
        {
            // NavMeshAgent hızını Animator'a aktar
            float speed = _agent.velocity.magnitude;
            _animator.SetFloat(Speed, speed);
        }

        private void InitializeComponents()
        {
            _agent = GetComponent<NavMeshAgent>();
            if (_agent == null)
            {
                Debug.LogError($"{gameObject.name} için NavMeshAgent bileşeni bulunamadı!");
            }

            _animator = GetComponentInChildren<Animator>();
            if (_animator == null)
            {
                Debug.LogError($"{gameObject.name} için Animator bileşeni bulunamadı!");
            }

            GameObject holeObject = GameObject.FindGameObjectWithTag("Player");
            if (holeObject != null)
            {
                _holeTransform = holeObject.transform;
            }
            else
            {
                Debug.LogError("Sahnede 'Player' etiketiyle işaretlenmiş bir nesne bulunamadı!");
            }
        }

        private void HandleIdleState()
        {
            _idleTimer += Time.deltaTime;

            if (_idleTimer >= _idleDuration)
            {
                _isIdle = false;
                _animator.SetBool(IsIdle, false);
                ChooseRandomTarget();
            }
        }

        private void HandleMovement()
        {
            if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
            {
                if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
                {
                    SetIdleState();
                }
            }
        }

        private void CheckHoleProximity()
        {
            float distanceToHole = Vector3.Distance(transform.position, _holeTransform.position);

            if (distanceToHole <= holeDetectionRadius)
            {
                _agent.speed = 5f;
                UpdateAnimationState(isRunning: true, isWalking: false);
            }
            else
            {
                _agent.speed = 2f;
                UpdateAnimationState(isRunning: false, isWalking: true);
            }
        }

        private void SetIdleState()
        {
            _isIdle = true;
            _idleTimer = 0f;
            _idleDuration = Random.Range(idleDurationMin, idleDurationMax);

            UpdateAnimationState(isIdle: true, isWalking: false, isRunning: false);
            _agent.ResetPath();
        }

        private void ChooseRandomTarget()
        {
            Vector3 randomDirection = new Vector3(
                Random.Range(-30f, 30f),
                0f,
                Random.Range(-30f, 30f)
            );
            Vector3 targetPosition = transform.position + randomDirection;

            if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            {
                _agent.SetDestination(hit.position);
            }
        }

        private void UpdateAnimationState(bool isIdle = false, bool isWalking = false, bool isRunning = false)
        {
            _animator.SetBool(IsIdle, isIdle);
            _animator.SetBool(IsWalking, isWalking);
            _animator.SetBool(IsRunning, isRunning);
        }
    }
}
