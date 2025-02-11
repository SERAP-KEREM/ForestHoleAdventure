using _Main._Hole;
using _Main._Level;
using _Main._UI;
using DG.Tweening;
using SerapKeremGameTools._Game._AudioSystem;
using SerapKeremGameTools._Game._SaveLoadSystem;
using SerapKeremGameTools._Game._Singleton;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Main._Managers
{
    /// <summary>
    /// Manages game flow, levels, UI, audio, and game state. Handles level transitions and music.
    /// </summary>
    public class GameManager : MonoSingleton<GameManager>
    {
        #region Serialized Fields

        [Header("Level Settings")]
        [SerializeField, Tooltip("Array of level data for game levels.")]
        private LevelData[] _levelDatas;

        [SerializeField, Tooltip("Index of the current level.")]
        private int _currentLevelIndex = 0;

        [SerializeField, Tooltip("Reference to the active Level Generator.")]
        private LevelGenerator _activeLevelGenerator;

        [Header("Audio")]
        [SerializeField, Tooltip("Name of the background music clip to play.")]
        private string backgroundMusicName = "BackgroundMusic";

        [SerializeField, Tooltip("Reference to the AudioManager.")]
        private AudioManager _audioManager;

        [Header("Game References")]
        [SerializeField, Tooltip("Reference to the HoleController.")]
        private HoleController _holeController;

        [SerializeField, Tooltip("Reference to the HoleManager for hole size and behavior.")]
        private HoleManager _holeManager;

        [SerializeField, Tooltip("Reference to the Gameplay UI for score and timer updates.")]
        private GameplayUI _gameplayUI;

        [SerializeField, Tooltip("Reference to the UIManager for UI management.")]
        private UIManager _uiManager;

        [SerializeField, Tooltip("Reference to the HoleCameraController to adjust the camera for hole growth.")]
        private HoleCameraController _cameraController;

        #endregion

        #region Private Variables

        private bool _isInitialized = false;

        #endregion

        #region Public Properties

        public HoleController GetHoleController() => _holeController;
        public HoleManager GetHoleManager() => _holeManager;
        public GameplayUI GetGameplayUI() => _gameplayUI;
        public UIManager GetUIManager() => _uiManager;
        public HoleCameraController GetHoleCameraController() => _cameraController;

        #endregion

        #region Unity Callbacks

        /// <summary>
        /// Called when the object is awake. Initializes the game by setting up the necessary components.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
        
        }

        /// <summary>
        /// Called at the start of the game. Starts the initialization coroutine and plays background music.
        /// </summary>
        private void Start()
        {
            InitializeGame();
            StartCoroutine(InitializeGameCoroutine());
            PlayBackgroundMusic();
            ApplySavedMusicVolume();
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes the game by loading saved data and setting up the audio manager.
        /// </summary>
        private void InitializeGame()
        {
            _currentLevelIndex = LoadManager.LoadData<int>("CurrentLevel", 0);
            _audioManager = AudioManager.Instance;

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        /// <summary>
        /// Coroutine that initializes game components with a slight delay.
        /// </summary>
        private IEnumerator InitializeGameCoroutine()
        {
            yield return new WaitForSeconds(0.1f);
            FindManagers();
            SetupCurrentLevel();
            _isInitialized = true;
        }

        #endregion

        #region Level Management

        /// <summary>
        /// Sets up the current level by initializing the level manager, UI, and level generator.
        /// </summary>
        private void SetupCurrentLevel()
        {
            if (_currentLevelIndex >= _levelDatas.Length)
            {
                _currentLevelIndex = 0;
                SaveManager.SaveData("CurrentLevel", 0);
            }

            _uiManager?.InitializeUI();
            LevelManager.Instance?.InitializeLevel(_levelDatas[_currentLevelIndex]);

            SetupLevelGenerator();
        }

        /// <summary>
        /// Sets up the level generator for the current level.
        /// </summary>
        private void SetupLevelGenerator()
        {
            if (_activeLevelGenerator != null)
            {
                Destroy(_activeLevelGenerator.gameObject);
            }

            LevelData currentLevelData = _levelDatas[_currentLevelIndex];
            if (currentLevelData.LevelGeneratorPrefab != null)
            {
                _activeLevelGenerator = Instantiate(
                    currentLevelData.LevelGeneratorPrefab,
                    Vector3.zero,
                    Quaternion.identity
                );
            }
            else
            {
                Debug.LogWarning($"Level {currentLevelData.LevelNumber} i�in LevelGeneratorPrefab atanmad?!");
            }
        }

        /// <summary>
        /// Marks the level as completed and progresses to the next level.
        /// </summary>
        public void LevelCompleted()
        {
            _currentLevelIndex++;
            SaveManager.SaveData("CurrentLevel", _currentLevelIndex);
        }

        /// <summary>
        /// Restarts the current level by reloading the scene.
        /// </summary>
        public void RestartLevel()
        {
            DOTween.KillAll();
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        /// <summary>
        /// Moves to the next level by completing the current one and restarting the scene.
        /// </summary>
        public void NextLevel()
        {
            LevelCompleted();
            RestartLevel();
        }

        #endregion

        #region Audio Management

        /// <summary>
        /// Plays the background music.
        /// </summary>
        public void PlayBackgroundMusic()
        {
            _audioManager?.PlayAudio(backgroundMusicName, true);
        }

        /// <summary>
        /// Applies the saved volume setting to all audio sources in the scene.
        /// </summary>
        private void ApplySavedMusicVolume()
        {
            float savedVolume = LoadManager.LoadData<float>("MusicVolume", 1f);

            AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
            foreach (var source in audioSources)
            {
                source.volume = savedVolume;
            }
        }

        #endregion

        #region Scene Management

        /// <summary>
        /// Called when a scene is loaded. Initializes the game for the newly loaded scene.
        /// </summary>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            StartCoroutine(OnSceneLoadedCoroutine());
        }

        /// <summary>
        /// Coroutine that runs after a scene is loaded, initializing the necessary game components.
        /// </summary>
        private IEnumerator OnSceneLoadedCoroutine()
        {
            yield return new WaitForSeconds(0.2f);
            _isInitialized = false;
            FindManagers();
            yield return new WaitForSeconds(0.1f);
            SetupCurrentLevel();
            _isInitialized = true;
        }

        #endregion

        #region Utility

        /// <summary>
        /// Finds the necessary managers in the scene.
        /// </summary>
        private void FindManagers()
        {
            _uiManager = FindObjectOfType<UIManager>();

            if (LevelManager.Instance == null || _uiManager == null)
            {
                Debug.LogError("Essential managers are missing!");
            }
        }

        /// <summary>
        /// Called when the object is destroyed. Unsubscribes from scene loading events and kills active tweens.
        /// </summary>
        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            DOTween.KillAll();
        }

        #endregion
    }
}
