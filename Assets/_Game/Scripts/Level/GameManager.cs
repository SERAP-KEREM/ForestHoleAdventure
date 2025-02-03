using UnityEngine;
using _Main._Level;
using _Main._UI;
using SerapKeremGameTools._Game._AudioSystem;
using SerapKeremGameTools._Game._Singleton;

namespace _Main.Managers
{
    public class GameManager : MonoSingleton<GameManager>
    {

        [Header("References")]
        [SerializeField] private LevelManager _levelManager;
        [SerializeField] private UIManager _uiManager;

        [Header("Level Settings")]
        [SerializeField] private LevelData[] _levelDatas;
        private int _currentLevelIndex = 0;

        [Header("Audio")]
        [SerializeField] private string backgroundMusicName = "BackgroundMusic";
        private AudioManager _audioManager;

        protected override void Awake()
        {
            base.Awake();
            InitializeAudio();
        }

        private void Start()
        {
            InitializeGame();
        }
        private void InitializeAudio()
        {
            _audioManager = AudioManager.Instance;
            if (_audioManager != null)
            {
                // Oyun ba?lad???nda müzi?i çal
                PlayBackgroundMusic();
            }
        }
        public void PlayBackgroundMusic()
        {
            _audioManager?.PlayAudio(backgroundMusicName);
        }

        public void PauseMusic()
        {
            _audioManager?.PauseAllAudio();
        }

        public void ResumeMusic()
        {
            _audioManager?.ResumeAllAudio();
        }
        private void InitializeGame()
        {
            // PlayerPrefs'ten kay?tl? level'? al
            _currentLevelIndex = PlayerPrefs.GetInt("CurrentLevel", 0);
            LoadCurrentLevel();
        }

        public void LoadCurrentLevel()
        {
            if (_levelManager != null)
            {
                _levelManager.InitializeLevel(_levelDatas[_currentLevelIndex]);
            }
        }

        public void LevelCompleted()
        {
            _currentLevelIndex++;
            if (_currentLevelIndex >= _levelDatas.Length)
            {
                _currentLevelIndex = 0; // Veya son level'da kal
            }
            PlayerPrefs.SetInt("CurrentLevel", _currentLevelIndex);
            PlayerPrefs.Save();
        }

        public void RestartLevel()
        {
            LoadCurrentLevel();
        }

        public void NextLevel()
        {
            _currentLevelIndex++;
            if (_currentLevelIndex >= _levelDatas.Length)
            {
                _currentLevelIndex = 0;
            }
            LoadCurrentLevel();
        }
    }
}