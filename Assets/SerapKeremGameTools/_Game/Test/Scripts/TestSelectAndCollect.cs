using SerapKeremGameTools._Game._InputSystem;
using SerapKeremGameTools.Game._Interfaces;
using UnityEngine;

namespace SerapKeremGameTools.InputSystem
{
    /// <summary>
    /// This class handles the behavior for objects that can be selected and collected. 
    /// It implements ISelectable and ICollectable interfaces.
    /// </summary>
    public class TestSelectAndCollect : MonoBehaviour, ISelectable, ICollectable
    {
        private Renderer objectRenderer;
        private Color originalColor;

        /// <summary>
        /// Initializes the object by storing its original color and getting its Renderer component.
        /// </summary>
        private void Awake()
        {
            objectRenderer = GetComponent<Renderer>();
            if (objectRenderer != null)
            {
                originalColor = objectRenderer.material.color;
            }
        }

        /// <summary>
        /// Called when the object is selected. Changes the object's color to green.
        /// </summary>
        public void Select()
        {
#if UNITY_EDITOR
            Debug.Log($"{gameObject.name} has been selected!");
#endif
            if (objectRenderer != null)
            {
                objectRenderer.material.color = Color.green; // Changes the color to green when selected
            }
        }

        /// <summary>
        /// Called when the object is deselected. Restores the object's original color.
        /// </summary>
        public void DeSelect()
        {
#if UNITY_EDITOR
            Debug.Log($"{gameObject.name} has been deselected!");
#endif
            if (objectRenderer != null)
            {
                objectRenderer.material.color = originalColor; // Restores the original color when deselected
            }
        }

        /// <summary>
        /// Called when the object is collected. Destroys the object after a short delay.
        /// </summary>
        public void Collect()
        {
#if UNITY_EDITOR
            Debug.Log($"{gameObject.name} has been collected!");
#endif
            Destroy(gameObject, 0.5f); // Destroys the object after a 0.5 second delay
        }
    }
}
