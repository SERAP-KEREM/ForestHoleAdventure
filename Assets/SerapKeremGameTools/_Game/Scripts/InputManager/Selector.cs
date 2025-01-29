using SerapKeremGameTools._Game._Singleton;
using SerapKeremGameTools.Game._Interfaces;
using UnityEngine;

namespace SerapKeremGameTools._Game._InputSystem
{
    /// <summary>
    /// Manages object selection and deselection based on player input.
    /// </summary>
    public class Selector : MonoSingleton<Selector>
    {
        /// <summary>
        /// The maximum distance for raycasting to detect selectable objects.
        /// </summary>
        [Tooltip("The maximum distance for raycasting to detect selectable objects.")]
        public float raycastLength = 10f;

        [Tooltip("The currently selected object implementing ISelectable.")]
        private ISelectable selectedObject;

        /// <summary>
        /// Ensures base initialization.
        /// </summary>
        protected void Awake()
        {
            base.Awake();
        }

        /// <summary>
        /// Subscribes to player input events.
        /// </summary>
        private void OnEnable()
        {
            if (PlayerInput.Instance == null)
            {
#if UNITY_EDITOR
                Debug.LogError("PlayerInput reference is missing!");
#endif
                return;
            }

            PlayerInput.Instance.OnMouseDownEvent.AddListener(SelectObject);
            PlayerInput.Instance.OnMouseUpEvent.AddListener(DeselectObject);
        }

        /// <summary>
        /// Unsubscribes from player input events.
        /// </summary>
        private void OnDisable()
        {
            if (PlayerInput.Instance != null)
            {
                PlayerInput.Instance.OnMouseDownEvent.RemoveListener(SelectObject);
                PlayerInput.Instance.OnMouseUpEvent.RemoveListener(DeselectObject);
            }
        }

        /// <summary>
        /// Attempts to select an object under the mouse pointer.
        /// </summary>
        private void SelectObject()
        {
            Ray ray = Camera.main.ScreenPointToRay(PlayerInput.Instance.MousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, raycastLength))
            {
                selectedObject = hit.collider.GetComponent<ISelectable>();
                selectedObject?.Select();
            }
        }

        /// <summary>
        /// Deselects the currently selected object and triggers collection if applicable.
        /// </summary>
        private void DeselectObject()
        {
            if (selectedObject != null)
            {
                selectedObject.DeSelect();

                if (selectedObject is ICollectable collectable)
                {
                    collectable.Collect();
                }

                selectedObject = null;
            }
        }
    }
}
