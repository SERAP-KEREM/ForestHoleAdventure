using UnityEngine;
using Cinemachine;
using DG.Tweening;
using TriInspector;
using SerapKeremGameTools._Game._Singleton;

namespace _Main._Hole
{
    /// <summary>
    /// Camera controller to handle hole camera's position and transition based on the hole size.
    /// </summary>
     [DeclareFoldoutGroup("Camera Settings", Title = "Camera Settings")]
    [DeclareFoldoutGroup("Follow Settings", Title = "Follow Settings")]
    [DeclareFoldoutGroup("Transition Settings", Title = "Transition Settings")]
    public class HoleCameraController : MonoSingleton<HoleCameraController>
    {
        #region Camera Settings

        [Group("Camera Settings")]
        [SerializeField, PropertyTooltip("The virtual camera that follows the hole.")]
        private CinemachineVirtualCamera _virtualCamera;

        #endregion

        #region Follow Settings

        [Group("Follow Settings")]
        [SerializeField, PropertyTooltip("Initial offset for the camera's position.")]
        private Vector3 _initialOffset = new Vector3(0f, 15f, -5f);

        [Group("Follow Settings")]
        [SerializeField, PropertyTooltip("Maximum offset for the camera's position.")]
        private Vector3 _maxOffset = new Vector3(0f, 25f, -15f);

        #endregion

        #region Transition Settings

        [Group("Transition Settings")]
        [SerializeField, PropertyTooltip("Duration of the camera transition.")]
        private float _transitionDuration = 0.5f;

        [Group("Transition Settings")]
        [SerializeField, PropertyTooltip("Ease type for the camera transition.")]
        private Ease _transitionEase = Ease.InOutQuad;

        #endregion

        private CinemachineTransposer _transposer;
        private float _initialHoleSize;
        private float _maxHoleSize;
        private Tween _currentCameraTween;

        protected override void Awake()
        {
            base.Awake();
            // Ensure virtual camera is assigned
            if (_virtualCamera == null)
            {
                _virtualCamera = GetComponent<CinemachineVirtualCamera>();
            }

            // Get Cinemachine Transposer component
            _transposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();

            if (_transposer == null)
            {
                Debug.LogError("Cinemachine Transposer is missing!");
            }
        }

        /// <summary>
        /// Initialize the camera with initial and max hole sizes.
        /// </summary>
        /// <param name="initialHoleSize">The initial size of the hole.</param>
        /// <param name="maxHoleSize">The maximum size of the hole.</param>
        public void Initialize(float initialHoleSize, float maxHoleSize)
        {
            _initialHoleSize = initialHoleSize;
            _maxHoleSize = maxHoleSize;

            if (_transposer != null)
            {
                _transposer.m_FollowOffset = _initialOffset;
            }
        }

        /// <summary>
        /// Updates the camera position based on the current hole size.
        /// </summary>
        /// <param name="currentHoleSize">The current size of the hole.</param>
        public void UpdateCameraPosition(float currentHoleSize)
        {
            if (_transposer == null) return;

            // Kill any ongoing tween animation
            _currentCameraTween?.Kill();

            // Get normalized position between initial and max hole size
            float t = Mathf.InverseLerp(_initialHoleSize, _maxHoleSize, currentHoleSize);

            // Calculate target camera offset based on hole size
            Vector3 targetOffset = Vector3.Lerp(_initialOffset, _maxOffset, t);

            // Create a new tween to smoothly move the camera
            _currentCameraTween = DOTween.To(
                () => _transposer.m_FollowOffset,
                x => _transposer.m_FollowOffset = x,
                targetOffset,
                _transitionDuration
            )
            .SetEase(_transitionEase)
            .SetUpdate(true);
        }


    }
}
