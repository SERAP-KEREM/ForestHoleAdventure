using UnityEngine;

namespace SerapKeremGameTools._Game._PopUpSystem
{
    /// <summary>
    /// Manages a pop-up displaying an icon.
    /// </summary>
    public class PopUpIcon : PopUp
    {
        [Header("Icon Settings")]
        [Tooltip("The SpriteRenderer component used to display the pop-up icon.")]
        [SerializeField] private SpriteRenderer spriteRenderer;

        protected override void Awake()
        {
            base.Awake();
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }
        }

        /// <summary>
        /// Initializes the pop-up with the specified icon and scale.
        /// </summary>
        /// <param name="args">Expected: a Sprite and an optional float for scale multiplier.</param>
        public override void Initialize(params object[] args)
        {
            if (args.Length == 0 || !(args[0] is Sprite sprite))
            {
                Debug.LogError("PopUpIcon: Invalid arguments for initialization.", this);
                return;
            }

            spriteRenderer.sprite = sprite;

            if (args.Length > 1 && args[1] is float scaleMultiplier)
            {
                transform.localScale = initialScale * scaleMultiplier;
            }

            StartCoroutine(PlayScaleAnimation());
        }

        /// <summary>
        /// Resets the pop-up icon and its properties.
        /// </summary>
        public override void ResetProperties()
        {
            base.ResetProperties();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = null;
            }
        }
    }
}