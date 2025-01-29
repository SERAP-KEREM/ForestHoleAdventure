using UnityEngine;
using SerapKeremGameTools._Game._Singleton;
using System.Collections;
using SerapKeremGameTools._Game._objectPool;

namespace SerapKeremGameTools._Game._PopUpSystem
{
    /// <summary>
    /// Abstract base class for managing pop-up objects in the scene.
    /// Provides core functionality for handling animations, pooling, and object lifecycle.
    /// Inherits from MonoSingleton to ensure a single instance per type.
    /// </summary>
    /// <typeparam name="T">The type of the pop-up object (e.g., text, icon).</typeparam>
    public abstract class PopUpManager<T, TManager> : MonoSingleton<TManager>
        where T : PopUp
        where TManager : PopUpManager<T, TManager>
    {
        [Header("Pool Settings")]
        [SerializeField, Tooltip("The initial pool size for pop-up objects.")]
        protected int poolSize = 10;

        [Header("Animation Settings")]
        [SerializeField, Tooltip("Duration of the animation.")]
        protected float animationDuration = 0.5f;

        [SerializeField, Tooltip("Delay before the pop-up disappears.")]
        protected float hideDelay = 1f;

        [SerializeField, Tooltip("Height for bounce animations.")]
        protected float bounceHeight = 2f;

        [SerializeField, Tooltip("Number of bounces for bounce animations.")]
        protected int bounceCount = 3;

        [SerializeField, Tooltip("Offset for slide animations.")]
        protected Vector3 slideOffset = new Vector3(0, 2, 0);

        protected ObjectPool<PopUp> popUpPool;

        /// <summary>
        /// Handles common initialization logic.
        /// </summary>
        protected virtual void Awake()
        {
            // Base implementation can be extended by child classes.
        }

        /// <summary>
        /// Handles animation logic specific to the pop-up object type.
        /// </summary>
        protected abstract IEnumerator HandleAnimation(T popUpObject, float duration, PopUpAnimationType animationType);


        /// <summary>
        /// Returns the pop-up icon to the pool after a specified delay.
        /// </summary>
        protected virtual IEnumerator ReturnPopUpObjectAfterDelay(PopUp popUp, float delay)
        {
            yield return new WaitForSeconds(delay);
            popUpPool.ReturnObject(popUp);
        }
    }
}