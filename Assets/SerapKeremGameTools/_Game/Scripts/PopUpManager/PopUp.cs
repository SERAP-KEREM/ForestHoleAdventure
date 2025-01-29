using System.Collections;
using UnityEngine;

namespace SerapKeremGameTools._Game._PopUpSystem
{
    /// <summary>
    /// Base class for managing pop-ups with common functionalities like initialization and resetting.
    /// </summary>
    public abstract class PopUp : MonoBehaviour
    {
        [Header("Pop-Up Settings")]
        [Tooltip("Duration for the pop-up to scale in and out.")]
        [SerializeField] private float scaleDuration = 0.3f;

        [Tooltip("Scale multiplier for pop-up animation.")]
        [Range(0.5f, 2f)]
        [SerializeField] private float scaleMultiplier = 1.2f;

        protected Vector3 initialScale;

        protected virtual void Awake()
        {
            initialScale = transform.localScale;
        }

        /// <summary>
        /// Initializes the pop-up with specific data.
        /// Must be implemented in derived classes.
        /// </summary>
        public abstract void Initialize(params object[] args);

        /// <summary>
        /// Resets the pop-up to its default state.
        /// </summary>
        public virtual void ResetProperties()
        {
            StopAllCoroutines();
            transform.localScale = initialScale;           
        }

        /// <summary>
        /// Plays a scaling animation to emphasize the pop-up.
        /// </summary>
        protected IEnumerator PlayScaleAnimation()
        {
            float elapsedTime = 0f;
            while (elapsedTime < scaleDuration)
            {
                float scale = Mathf.Lerp(1f, scaleMultiplier, elapsedTime / scaleDuration);
                transform.localScale = initialScale * scale;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            elapsedTime = 0f;
            while (elapsedTime < scaleDuration)
            {
                float scale = Mathf.Lerp(scaleMultiplier, 1f, elapsedTime / scaleDuration);
                transform.localScale = initialScale * scale;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.localScale = initialScale;
        }
    }
}