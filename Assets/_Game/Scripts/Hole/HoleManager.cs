using _Main._Level;
using _Main._Objects;
using SerapKeremGameTools._Game._Singleton;
using UnityEngine;

namespace _Main._Hole
{
    /// <summary>
    /// Manages the hole's interactions with collectible objects in the game.
    /// </summary>
    public class HoleManager : MonoSingleton<HoleManager>
    {
        #region Serialized Fields

        [Header("Hole Settings")]
        [SerializeField, Tooltip("Transform of the hole object.")]
        private Transform _holeTransform;



        #endregion

        protected override void Awake()
        {
            base.Awake();
        }
        /// <summary>
        /// Called when the hole collides with a collectible item.
        /// </summary>
        /// <param name="other">The collider of the object the hole collided with.</param>
        private void OnTriggerEnter(Collider other)
        {
            // Check if the collided object is a collectible item
            CollectibleItem collectible = other.GetComponent<CollectibleItem>();
            if (collectible != null && !collectible.IsCollected)
            {
                // Get the hole and object size
                float holeSize = _holeTransform.localScale.x;
                float objectSize = collectible.Size;

                // Check if the hole is large enough to collect the object
                if (holeSize >= objectSize * 0.9f)
                {
                    LevelManager.Instance.AddScore(collectible.Score);
                    collectible.Collect(_holeTransform);

                    Debug.Log($"Collected: {other.gameObject.name}, " +
                             $"Hole Size: {holeSize}, Object Size: {objectSize}");
                }
                else
                {
                    Debug.Log($"Object too big: Hole size {holeSize}, Object size {objectSize}");
                }
            }
        }

        /// <summary>
        /// Updates the hole's size and position.
        /// </summary>
        /// <param name="size">The new size for the hole.</param>
        public void SetHoleSize(float size)
        {
            // Update the scale of the hole
            Vector3 newScale = Vector3.one * size;
            newScale.y = _holeTransform.localScale.y; // Keep the y scale fixed
            _holeTransform.localScale = newScale;

            // Update the hole's position, keeping the y-coordinate at 0
            Vector3 position = _holeTransform.position;
            position.y = 0f;
            _holeTransform.position = position;
        }
    }
}
