using UnityEngine;
using _Main.Managers;

namespace _Main._UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameplayUI _gameplayUI;
        [SerializeField] private SettingsPanel _settingsPanel;
        [SerializeField] private WinPanel _winPanel;
        [SerializeField] private FailPanel _failPanel;

        private void Start()
        {
            InitializePanels();
        }

        private void InitializePanels()
        {
            _gameplayUI?.gameObject.SetActive(true);
            _settingsPanel?.Hide();
            _winPanel?.Hide();
            _failPanel?.Hide();
        }

        public void ShowSettings()
        {
            _settingsPanel?.Show();
        }

        public void HideSettings()
        {
            _settingsPanel?.Hide();
        }

        public void ShowWinPanel()
        {
            _gameplayUI?.gameObject.SetActive(false);
            _winPanel?.Show();
        }

        public void ShowFailPanel()
        {
            _gameplayUI?.gameObject.SetActive(false);
            _failPanel?.Show();
        }

        public void RestartLevel()
        {
            GameManager.Instance.RestartLevel();
            InitializePanels();
        }

        public void NextLevel()
        {
            GameManager.Instance.NextLevel();
            InitializePanels();
        }
    }
}