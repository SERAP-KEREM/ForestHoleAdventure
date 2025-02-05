using SerapKeremGameTools._Game._Singleton;
using SerapKeremGameTools.Game._Interfaces;
using UnityEngine;

namespace SerapKeremGameTools._Game._InputSystem
{
    public class Selector : MonoSingleton<Selector>
    {
        [SerializeField] private float raycastLength = 10f;
        private ISelectable selectedObject;
        private Camera _mainCamera;

        protected override void Awake()
        {
            base.Awake();
            _mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            if (PlayerInput.Instance == null)
            {
                Debug.LogError("PlayerInput reference is missing!");
                return;
            }

            PlayerInput.Instance.OnMouseDownEvent.AddListener(SelectObject);
            PlayerInput.Instance.OnMouseUpEvent.AddListener(DeselectObject);
        }

        private void OnDisable()
        {
            if (PlayerInput.Instance != null)
            {
                PlayerInput.Instance.OnMouseDownEvent.RemoveListener(SelectObject);
                PlayerInput.Instance.OnMouseUpEvent.RemoveListener(DeselectObject);
            }
        }

        private void SelectObject()
        {
            if (_mainCamera == null) return;

            Ray ray = _mainCamera.ScreenPointToRay(PlayerInput.Instance.MousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, raycastLength))
            {
                selectedObject = hit.collider.GetComponent<ISelectable>();
                selectedObject?.Select();
            }
        }

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