using UnityEngine;
using DG.Tweening;

namespace _Main._Objects
{
    /// <summary>
    /// Handles the behavior of collectible items in the game, including their collection animation and properties.
    /// </summary>
    public class CollectibleItem : MonoBehaviour
    {
        #region Serialized Fields

        [Header("Collection Animation Settings")]
        [SerializeField, Tooltip("The duration of the collection animation.")]
        private float _collectDuration = 2f;

        [SerializeField, Tooltip("The depth at which the item falls during collection animation.")]
        private float _fallDepth = 0.5f;

        [SerializeField, Tooltip("The duration for fading the collectible item.")]
        private float _fadeDuration = 1.5f;

        [Header("Object Settings")]
        [SerializeField, Tooltip("The type of collectible item, used to determine score.")]
        private CollectibleType _type;

        #endregion

        #region Private Variables

        private Material _material;
        private Color _originalColor;
        private float _size;
        private bool _isCollected;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the score value associated with this collectible item based on its type.
        /// </summary>
        public int Score => (int)_type;

        /// <summary>
        /// Gets the size of the collectible item.
        /// </summary>
        public float Size => _size;

        /// <summary>
        /// Indicates whether the item has been collected.
        /// </summary>
        public bool IsCollected => _isCollected;

        #endregion

        #region Unity Callbacks

        /// <summary>
        /// Called when the script is initialized. It calculates the size of the item and initializes its material.
        /// </summary>
        private void Awake()
        {
            CalculateSize();
            InitializeMaterial();
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Calculates the size of the collectible item based on its collider bounds.
        /// </summary>
        private void CalculateSize()
        {
            Bounds bounds = GetComponent<Collider>().bounds;
            _size = Mathf.Max(bounds.size.x, bounds.size.z) * 0.5f;

            Debug.Log($"Object {gameObject.name} calculated size: {_size}");
        }

        /// <summary>
        /// Initializes the material properties of the collectible item, enabling transparency and setup for fade effects.
        /// </summary>
        private void InitializeMaterial()
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                _material = renderer.material;
                _originalColor = _material.color;

                // Set the material to be transparent
                _material.SetFloat("_Surface", 1);
                _material.SetOverrideTag("RenderType", "Transparent");
                _material.SetInt("_ZWrite", 0);
                _material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                _material.EnableKeyword("_ALPHABLEND_ON");
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Handles the collection of the item, animating it towards the hole and then deactivating it.
        /// </summary>
        /// <param name="holeTransform">The transform of the hole where the collectible item should move.</param>
        public void Collect(Transform holeTransform)
        {
            if (!_isCollected)
            {
                _isCollected = true;

                // Animate the item moving towards the hole
                transform.DOMove(holeTransform.position, _collectDuration)
                    .SetEase(Ease.InQuad);

                // Animate the item scaling down to zero and deactivate it
                transform.DOScale(Vector3.zero, _collectDuration)
                    .SetEase(Ease.InQuad)
                    .OnComplete(() =>
                    {
                        gameObject.SetActive(false);
                        transform.localScale = Vector3.one; // Reset scale after deactivation
                    });
            }
        }

        #endregion

        #region Gizmos

        /// <summary>
        /// Draws a gizmo to visualize the size of the collectible item in the scene view when selected.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            if (Application.isPlaying)
            {
                Gizmos.color = Color.yellow;
                Vector3 size = new Vector3(_size, 0.1f, _size);
                Gizmos.DrawWireCube(transform.position, size);
            }
        }

        #endregion
    }
}
