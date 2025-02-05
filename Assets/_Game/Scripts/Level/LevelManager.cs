using UnityEngine;
using UnityEngine.Events;
using _Main._UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using DG.Tweening;
using _Main._Hole;
using TriInspector;
using SerapKeremGameTools._Game._Singleton;

namespace _Main._Level
{
    /// <summary>
    /// Handles the current level’s gameplay mechanics, including score tracking, timer, hole size growth, and level completion.
    /// </summary>
    public class LevelManager : MonoSingleton<LevelManager>
    {
        #region Serialized Fields

        [Group("Events")]
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
        protected override void Awake()
        {
            base.Awake();
        }
        /// <summary>
        /// Initializes the level by validating references and ensuring proper setup.
        /// </summary>
        

        #endregion

        #region Initialization

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

            if (HoleManager.Instance != null)
            {
                HoleManager.Instance.SetHoleSize(_currentLevel.InitialHoleSize);
            }

            if (GameplayUI.Instance != null)
            {
                GameplayUI.Instance.Initialize(
                    _currentLevel.LevelNumber,
                    _currentLevel.TargetScore,
                    _currentLevel.ScoreThresholds
                );
            
            }

            if (HoleCameraController.Instance != null)
            {
                HoleCameraController.Instance.Initialize(
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

            if (GameplayUI.Instance != null)
            {
                GameplayUI.Instance.UpdateScore(_currentScore);
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

                if (HoleManager.Instance != null)
                {
                    HoleManager.Instance.SetHoleSize(newSize);
                }

                if (HoleCameraController.Instance != null)
                {
                    HoleCameraController.Instance.UpdateCameraPosition(newSize);
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
            if (GameplayUI.Instance != null)
            {
                GameplayUI.Instance.UpdateTimer(_remainingTime);
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
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowWinPanel();
            }
        }

        /// <summary>
        /// Ends the level unsuccessfully, triggering the level failed event and showing the fail panel.
        /// </summary>
        private void LevelFailed()
        {
            _isGameActive = false;
            onLevelFailed?.Invoke();  // Trigger level failed event
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowFailPanel();
            }
        }

        #endregion
    }
}
