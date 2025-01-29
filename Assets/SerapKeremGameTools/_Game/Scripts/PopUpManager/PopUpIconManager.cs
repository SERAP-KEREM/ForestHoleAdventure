using UnityEngine;
using SerapKeremGameTools._Game._objectPool;
using System.Collections;

namespace SerapKeremGameTools._Game._PopUpSystem
{
    /// <summary>
    /// Manages the creation, animation, and pooling of icon-based pop-ups.
    /// </summary>
    public class PopUpIconManager : PopUpManager<PopUpIcon, PopUpIconManager>
    {
        [Header("Pop-Up Icon Settings")]
        [SerializeField, Tooltip("Prefab for the pop-up icon.")]
        private PopUpIcon popUpIconPrefab;


        /// <summary>
        /// Initialization logic for the icon manager.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            popUpPool = new ObjectPool<PopUp>(popUpIconPrefab, poolSize, transform);
        }

        /// <summary>
        /// Displays a pop-up icon at a specified position with a custom animation.
        /// </summary>
        /// <param name="position">Position where the icon appears.</param>
        /// <param name="sprite">Sprite to display.</param>
        /// <param name="customDuration">Custom animation duration.</param>
        /// <param name="animationType">Type of animation to use.</param>
        public void ShowPopUpIcon(Vector3 position, Sprite sprite, float customDuration, PopUpAnimationType animationType)
        {
            PopUpIcon popUpIcon = popUpPool.GetObject() as PopUpIcon;
            popUpIcon.transform.position = position;
            popUpIcon.Initialize(sprite);

            float duration = customDuration > 0 ? customDuration : animationDuration;

            StartCoroutine(HandleAnimation(popUpIcon, duration, animationType));
            StartCoroutine(ReturnPopUpObjectAfterDelay(popUpIcon, duration + hideDelay));
        }

        /// <summary>
        /// Handles animations for the pop-up icon.
        /// </summary>
        protected override IEnumerator HandleAnimation(PopUpIcon popUpIcon, float duration, PopUpAnimationType animationType)
        {
            switch (animationType)
            {
                case PopUpAnimationType.ScaleAndFade:
                    yield return ScaleAndFadeAnimation(popUpIcon, duration);
                    break;
                case PopUpAnimationType.SlideUp:
                    yield return SlideAnimation(popUpIcon, slideOffset, duration);
                    break;
                case PopUpAnimationType.Bounce:
                    yield return BounceAnimation(popUpIcon, duration);
                    break;
            }
        }

        /// <summary>
        /// Animates scaling and fading of the pop-up icon.
        /// </summary>
        private IEnumerator ScaleAndFadeAnimation(PopUpIcon popUpIcon, float duration)
        {
            Vector3 startScale = Vector3.zero;
            Vector3 endScale = Vector3.one;
            float elapsedTime = 0f;

            while (elapsedTime < animationDuration)
            {
                float t = elapsedTime / animationDuration;
                popUpIcon.transform.localScale = Vector3.Lerp(startScale, endScale, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(duration);

            elapsedTime = 0f;
            while (elapsedTime < animationDuration)
            {
                float t = elapsedTime / animationDuration;
                popUpIcon.transform.localScale = Vector3.Lerp(endScale, startScale, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        /// <summary>
        /// Animates sliding of the pop-up icon.
        /// </summary>
        private IEnumerator SlideAnimation(PopUpIcon popUpIcon, Vector3 offset, float duration)
        {
            Vector3 startPosition = popUpIcon.transform.position;
            Vector3 targetPosition = startPosition + offset;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = Mathf.SmoothStep(0, 1, elapsedTime / duration);
                popUpIcon.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            popUpIcon.transform.position = targetPosition;
        }

        /// <summary>
        /// Animates bouncing of the pop-up icon.
        /// </summary>
        private IEnumerator BounceAnimation(PopUpIcon popUpIcon, float duration)
        {
            Vector3 startPosition = popUpIcon.transform.position;
            float elapsedTime = 0f;

            for (int i = 0; i < bounceCount; i++)
            {
                float t = 0f;
                while (t < 1f)
                {
                    float yOffset = Mathf.Sin(t * Mathf.PI) * bounceHeight;
                    popUpIcon.transform.position = startPosition + new Vector3(0, yOffset, 0);
                    t += Time.deltaTime / (duration / bounceCount);
                    yield return null;
                }
            }

            popUpIcon.transform.position = startPosition;
        }
    }
}