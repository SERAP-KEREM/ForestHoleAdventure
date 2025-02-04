using UnityEngine;
using UnityEngine.Events;
using _Main._UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using DG.Tweening;
using _Main._Hole;

namespace _Main._Level
{
    /// <summary>
    /// Handles the current level’s gameplay mechanics, including score tracking, timer, hole size growth, and level completion.
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        #region Serialized Fields

        [Header("References")]
        [SerializeField, Tooltip("Reference to the HoleManager for hole size and behavior.")] private HoleManager _holeManager;
        [SerializeField, Tooltip("Reference to the Gameplay UI for score and timer updates.")] private GameplayUI _gameplayUI;
        [SerializeField, Tooltip("Reference to the UIManager for UI management.")] private UIManager _uiManager;
        [SerializeField, Tooltip("Reference to the HoleCameraController to adjust the camera for hole growth.")] private HoleCameraController _cameraController;

        [Header("Events")]
        public UnityEvent onLevelComplete;
        public UnityEvent onLevelFailed;

        #endregion

        #region Private Variables

        private LevelData _currentLevel;
        private int _currentScore;
        private float _remainingTime;
        private int _currentThresholdIndex;
        private bool _isGameActive;

        #endregion

        #region Unity Callbacks

        /// <summary>
        /// Initializes the level by validating references and ensuring proper setup.
        /// </summary>
        private void Start()
        {
            ValidateReferences();
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Validates that all necessary references are assigned in the inspector. Logs errors if any reference is missing.
        /// </summary>
        private void ValidateReferences()
        {
            if (_holeManager == null) _holeManager = FindObjectOfType<HoleManager>();
            if (_gameplayUI == null) _gameplayUI = FindObjectOfType<GameplayUI>();
            if (_uiManager == null) _uiManager = FindObjectOfType<UIManager>();
            if (_cameraController == null) _cameraController = FindObjectOfType<HoleCameraController>();

            // Log errors if any reference is missing
            if (_holeManager == null) Debug.LogError("HoleManager is missing!");
            if (_gameplayUI == null) Debug.LogError("GameplayUI is missing!");
            if (_uiManager == null) Debug.LogError("UIManager is missing!");
            if (_cameraController == null) Debug.LogError("HoleCameraController is missing!");
        }

        /// <summary>
        /// Initializes the level using the provided LevelData, setting the initial score, timer, and hole size.
        /// </summary>
        /// <param name="levelData">The data representing the current level.</param>
        public void InitializeLevel(LevelData levelData)
        {
            if (levelData == null)
            {
                Debug.LogError("LevelData is null!");
                return;
            }

            _currentLevel = levelData;
            _currentScore = 0;
            _remainingTime = _currentLevel.LevelTime;
            _currentThresholdIndex = 0;
            _isGameActive = true;
            Time.timeScale = 1f;

            if (_holeManager != null)
            {
                _holeManager.SetHoleSize(_currentLevel.InitialHoleSize);
            }

            if (_gameplayUI != null)
            {
                _gameplayUI.Initialize(
                    _currentLevel.LevelNumber,
                    _currentLevel.TargetScore,
                    _currentLevel.ScoreThresholds
                );
            
            }

            if (_cameraController != null)
            {
                _cameraController.Initialize(
                    _currentLevel.InitialHoleSize,
                    _currentLevel.MaxHoleSize
                );
            }
        }

        #endregion

        #region Gameplay

        /// <summary>
        /// Updates the game’s timer and checks for level failure when the time runs out.
        /// </summary>
        private void Update()
        {
            if (!_isGameActive) return;
            UpdateTimer();
        }

        /// <summary>
        /// Adds the specified points to the score and checks if the level has been completed.
        /// </summary>
        /// <param name="points">The points to add to the score.</param>
        public void AddScore(int points)
        {
            if (!_isGameActive) return;

            _currentScore += points;
            CheckGrowthThreshold();

            if (_gameplayUI != null)
            {
                _gameplayUI.UpdateScore(_currentScore);
            }

            if (_currentScore >= _currentLevel.TargetScore)
            {
                LevelComplete();
            }
        }

        /// <summary>
        /// Checks if the score has surpassed a threshold, and if so, grows the hole and adjusts the camera.
        /// </summary>
        private void CheckGrowthThreshold()
        {
         
            if (_currentThresholdIndex >= _currentLevel.ScoreThresholds.Length) return;
       
            if (_currentScore >= _currentLevel.ScoreThresholds[_currentThresholdIndex])
            {
                float newSize = _currentLevel.InitialHoleSize +
                    (_currentLevel.GrowthAmount * (_currentThresholdIndex + 1));

                if (_holeManager != null)
                {
                    _holeManager.SetHoleSize(newSize);
                }

                if (_cameraController != null)
                {
                    _cameraController.UpdateCameraPosition(newSize);
                }

                _currentThresholdIndex++;
            }
        }

        /// <summary>
        /// Updates the remaining time and checks if the level has failed due to time running out.
        /// </summary>
        private void UpdateTimer()
        {
            _remainingTime -= Time.deltaTime;
            if (_gameplayUI != null)
            {
                _gameplayUI.UpdateTimer(_remainingTime);
            }

            if (_remainingTime <= 0)
            {
                _remainingTime = 0;
                LevelFailed();
            }
        }

        #endregion

        #region Level Management

        /// <summary>
        /// Ends the level successfully, triggering the level completion event and showing the win panel.
        /// </summary>
        private void LevelComplete()
        {
            _isGameActive = false;
            onLevelComplete?.Invoke();  // Trigger level completion event
            if (_uiManager != null)
            {
                _uiManager.ShowWinPanel();
            }
        }

        /// <summary>
        /// Ends the level unsuccessfully, triggering the level failed event and showing the fail panel.
        /// </summary>
        private void LevelFailed()
        {
            _isGameActive = false;
            onLevelFailed?.Invoke();  // Trigger level failed event
            if (_uiManager != null)
            {
                _uiManager.ShowFailPanel();
            }
        }

        #endregion
    }
}
