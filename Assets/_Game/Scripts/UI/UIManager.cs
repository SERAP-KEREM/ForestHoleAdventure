using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using _Main._Hole;
using _Main._Managers;
using TriInspector;
using SerapKeremGameTools._Game._Singleton;

namespace _Main._UI
{
    [DeclareFoldoutGroup("Panels", Title = "Panels")]
    public class UIManager : MonoSingleton<UIManager>
    {
       
        [SerializeField, Group("Panels")] private GameplayUI _gameplayUI;
        [SerializeField, Group("Panels")] private SettingsPanel _settingsPanel;
        [SerializeField, Group("Panels")] private WinPanel _winPanel;
        [SerializeField, Group("Panels")] private FailPanel _failPanel;

    

        #region Initialization

        /// <summary>
        /// Initializes the UI Manager, checks required references.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            ValidateReferences();
            SetupPanels();
        }

        /// <summary>
        /// Checks for missing references and logs error messages.
        /// </summary>
        private void ValidateReferences()
        {
            if (_gameplayUI == null) Debug.LogError("GameplayUI is missing!");
            if (_winPanel == null) Debug.LogError("WinPanel is missing!");
            if (_failPanel == null) Debug.LogError("FailPanel is missing!");
            if (_settingsPanel == null) Debug.LogError("SettingsPanel is missing!");
        }

        #endregion

        #region Panel Management

        /// <summary>
        /// Disables all panels.
        /// </summary>
        private void SetupPanels()
        {
            if (_gameplayUI != null) _gameplayUI.gameObject.SetActive(false);
            if (_winPanel != null) _winPanel.gameObject.SetActive(false);
            if (_failPanel != null) _failPanel.gameObject.SetActive(false);
            if (_settingsPanel != null) _settingsPanel.gameObject.SetActive(false);
        }

        /// <summary>
        /// Initializes the UI by enabling the gameplay panel.
        /// </summary>
        public void InitializeUI()
        {
            SetupPanels();
            if (_gameplayUI != null)
            {
                _gameplayUI.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Hides the settings panel and enables hole controller.
        /// </summary>
        public void HideSettings()
        {
            if (_settingsPanel != null)
            {
                _settingsPanel.Hide();
                HoleController.Instance.EnableControl();
            }
        }

        #endregion

        #region Panels Show/Hide

        /// <summary>
        /// Displays the Win panel and disables hole controller.
        /// </summary>
        public void ShowWinPanel()
        {
            if (_winPanel != null)
            {
                _winPanel.gameObject.SetActive(true);
                _winPanel.Show();
                 HoleController.Instance.DisableControl();
            }
        }

        /// <summary>
        /// Displays the Fail panel and disables hole controller.
        /// </summary>
        public void ShowFailPanel()
        {
            if (_failPanel != null)
            {
                _failPanel.gameObject.SetActive(true);
                _failPanel.Show();
                HoleController.Instance.DisableControl();
            }
        }

        /// <summary>
        /// Displays the settings panel and disables hole controller.
        /// </summary>
        public void ShowSettings()
        {
            if (_settingsPanel != null)
            {
                _settingsPanel.Show();
                HoleController.Instance.DisableControl();
            }
        }

        #endregion

        #region Game Control

        /// <summary>
        /// Restarts the game.
        /// </summary>
        public void RestartGame()
        {
            DOTween.KillAll();
            Time.timeScale = 1f;
            GameManager.Instance.RestartLevel();
        }

        /// <summary>
        /// Moves to the next level.
        /// </summary>
        public void NextLevel()
        {
            DOTween.KillAll();
            Time.timeScale = 1f;
            GameManager.Instance.NextLevel();
        }

        #endregion
    }
}
