using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TriInspector;

namespace _Main._UI
{
    public class WinPanel : MonoBehaviour
    {
        [Group("UI Buttons")]
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private Button _restartButton;

        #region Initialization

        /// <summary>
        /// Initializes the WinPanel, sets up button listeners.
        /// </summary>
        private void Awake()
        {
            SetupButtons();
        }

        /// <summary>
        /// Sets up the button listeners for next level and restart actions.
        /// </summary>
        private void SetupButtons()
        {
            if (_nextLevelButton != null)
                _nextLevelButton.onClick.AddListener(() => UIManager.Instance.NextLevel());

            if (_restartButton != null)
                _restartButton.onClick.AddListener(() => UIManager.Instance.RestartGame());
        }

        #endregion

        #region UI Control

        /// <summary>
        /// Displays the win panel with an animation.
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, 0.3f)
                .SetEase(Ease.OutBack)
                .SetUpdate(true);
        }

        #endregion

        #region Cleanup

        /// <summary>
        /// Removes all button listeners to prevent memory leaks.
        /// </summary>
        private void OnDestroy()
        {
            if (_nextLevelButton != null)
                _nextLevelButton.onClick.RemoveAllListeners();
            if (_restartButton != null)
                _restartButton.onClick.RemoveAllListeners();
        }

        #endregion
    }
}
