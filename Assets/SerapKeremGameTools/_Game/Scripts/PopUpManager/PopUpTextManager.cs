using UnityEngine;
using System.Collections;
using SerapKeremGameTools._Game._objectPool;

namespace SerapKeremGameTools._Game._PopUpSystem
{
    /// <summary>
    /// Manages the creation, animation, and pooling of text-based pop-ups.
    /// Inherits from PopUpManager for shared behavior.
    /// </summary>
    public class PopUpTextManager : PopUpManager<PopUpText, PopUpTextManager>
    {
        [Header("Pop-Up Text Settings")]
        [SerializeField, Tooltip("Prefab for the pop-up text.")]
        private PopUpText popUpTextPrefab;

        [SerializeField, Tooltip("Scale applied to the pop-up text.")]
        private Vector3 popUpTextScale = Vector3.one;

        protected override void Awake()
        {
            base.Awake();
            popUpPool = new ObjectPool<PopUp>(popUpTextPrefab, poolSize, transform);
        }

        /// <summary>
        /// Displays a pop-up text at a specified position with custom animation.
        /// </summary>
        public void ShowPopUpText(Vector3 position, string text, float customDuration, PopUpAnimationType animationType)
        {
            PopUpText popUpText = popUpPool.GetObject() as PopUpText;
            popUpText.transform.position = position;
            popUpText.Initialize(text);
            popUpText.transform.localScale = popUpTextScale;

            float duration = customDuration > 0 ? customDuration : animationDuration;
            
            StartCoroutine(HandleAnimation(popUpText, duration, animationType));
            StartCoroutine(ReturnPopUpObjectAfterDelay(popUpText, duration + hideDelay));
        }

        /// <summary>
        /// Handles animations for the pop-up icon.
        /// </summary>
        protected override IEnumerator HandleAnimation(PopUpText popUpText, float duration, PopUpAnimationType animationType)
        {
            switch (animationType)
            {
                case PopUpAnimationType.ScaleAndFade:
                    yield return ScaleAndFadeAnimation(popUpText, duration);
                    break;
                case PopUpAnimationType.SlideUp:
                    yield return SlideAnimation(popUpText, slideOffset, duration);
                    break;
                case PopUpAnimationType.Bounce:
                    yield return BounceAnimation(popUpText, duration);
                    break;
            }
        }

        /// <summary>
        /// Animates scaling and fading of the pop-up icon.
        /// </summary>
        private IEnumerator ScaleAndFadeAnimation(PopUpText popUpText, float duration)
        {
            Vector3 startScale = Vector3.zero;
            Vector3 endScale = Vector3.one;
            float elapsedTime = 0f;

            while (elapsedTime < animationDuration)
            {
                float t = elapsedTime / animationDuration;
                popUpText.transform.localScale = Vector3.Lerp(startScale, endScale, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(duration);

            elapsedTime = 0f;
            while (elapsedTime < animationDuration)
            {
                float t = elapsedTime / animationDuration;
                popUpText.transform.localScale = Vector3.Lerp(endScale, startScale, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        /// <summary>
        /// Animates sliding of the pop-up icon.
        /// </summary>
        private IEnumerator SlideAnimation(PopUpText popUpText, Vector3 offset, float duration)
        {
            Vector3 startPosition = popUpText.transform.position;
            Vector3 targetPosition = startPosition + offset;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = Mathf.SmoothStep(0, 1, elapsedTime / duration);
                popUpText.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            popUpText.transform.position = targetPosition;
        }

        /// <summary>
        /// Animates bouncing of the pop-up icon.
        /// </summary>
        private IEnumerator BounceAnimation(PopUpText popUpText, float duration)
        {
            Vector3 startPosition = popUpText.transform.position;
            float elapsedTime = 0f;

            for (int i = 0; i < bounceCount; i++)
            {
                float t = 0f;
                while (t < 1f)
                {
                    float yOffset = Mathf.Sin(t * Mathf.PI) * bounceHeight;
                    popUpText.transform.position = startPosition + new Vector3(0, yOffset, 0);
                    t += Time.deltaTime / (duration / bounceCount);
                    yield return null;
                }
            }

            popUpText.transform.position = startPosition;
        }
    }
}