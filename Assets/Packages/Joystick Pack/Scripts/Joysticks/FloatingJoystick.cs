using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FloatingJoystick : Joystick
{
    private CanvasGroup _canvasGroup;
    private Canvas _canvas;
    private Camera _uiCamera;

    private void Awake()
    {
        SetupComponents();
    }

    public void Initialize()
    {
        SetupComponents();
        background.gameObject.SetActive(false);
    }

    private void SetupComponents()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
        {
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        _canvas = GetComponentInParent<Canvas>();
        if (_canvas != null)
        {
            _uiCamera = _canvas.worldCamera;
        }

        // Canvas'ın raycast hedeflerini görmezden gelmesini sağla
        if (_canvas != null)
        {
            var raycaster = _canvas.GetComponent<GraphicRaycaster>();
            if (raycaster != null)
            {
                raycaster.ignoreReversedGraphics = false;
            }
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
    }

    public void Enable()
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
    }

    public void Disable()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
        background.gameObject.SetActive(false);
    }
}