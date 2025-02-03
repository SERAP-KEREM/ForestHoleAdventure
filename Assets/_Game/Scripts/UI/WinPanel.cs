using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace _Main._UI
{
    public class WinPanel : MonoBehaviour
    {
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private Button _restartButton;

        private void Awake()
        {
            _nextLevelButton.onClick.AddListener(OnNextLevelClicked);
            _restartButton.onClick.AddListener(OnRestartClicked);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            transform.DOScale(Vector3.one, 0.3f).From(Vector3.zero).SetEase(Ease.OutBack);
        }

        public void Hide()
        {
            transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack)
                .OnComplete(() => gameObject.SetActive(false));
        }

        private void OnNextLevelClicked()
        {
            FindObjectOfType<UIManager>().NextLevel();
        }

        private void OnRestartClicked()
        {
            FindObjectOfType<UIManager>().RestartLevel();
        }
    }
}