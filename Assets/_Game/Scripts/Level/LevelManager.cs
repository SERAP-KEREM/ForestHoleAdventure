using UnityEngine;
using UnityEngine.Events;
using _Main.Hole;

using _Main.Managers;
using _Main._UI;

namespace _Main._Level
{
    public class LevelManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private HoleManager holeManager;
        [SerializeField] private GameplayUI gameplayUI;
        [SerializeField] private UIManager uiManager;

        private LevelData currentLevel;
        private int currentScore;
        private float remainingTime;
        private int currentThresholdIndex;
        private bool isGameActive;

        public void InitializeLevel(LevelData levelData)
        {
            currentLevel = levelData;
            currentScore = 0;
            remainingTime = currentLevel.LevelTime;
            currentThresholdIndex = 0;
            isGameActive = true;

            holeManager.SetHoleSize(currentLevel.InitialHoleSize);
            gameplayUI.Initialize(
                currentLevel.LevelNumber,
                currentLevel.TargetScore,
                currentLevel.ScoreThresholds
            );
        }

        private void Update()
        {
            if (!isGameActive) return;
            UpdateTimer();
        }

        public void AddScore(int points)
        {
            if (!isGameActive) return;

            currentScore += points;
            CheckGrowthThreshold();
            gameplayUI.UpdateScore(currentScore);

            if (currentScore >= currentLevel.TargetScore)
            {
                LevelComplete();
            }
        }

        private void CheckGrowthThreshold()
        {
            if (currentThresholdIndex >= currentLevel.ScoreThresholds.Length) return;

            if (currentScore >= currentLevel.ScoreThresholds[currentThresholdIndex])
            {
                float newSize = currentLevel.InitialHoleSize +
                    (currentLevel.GrowthAmount * (currentThresholdIndex + 1));
                holeManager.SetHoleSize(newSize);
                currentThresholdIndex++;
            }
        }

        private void UpdateTimer()
        {
            remainingTime -= Time.deltaTime;
            gameplayUI.UpdateTimer(remainingTime);

            if (remainingTime <= 0)
            {
                remainingTime = 0;
                LevelFailed();
            }
        }

        private void LevelComplete()
        {
            isGameActive = false;
            GameManager.Instance.LevelCompleted();
            uiManager.ShowWinPanel();
        }

        private void LevelFailed()
        {
            isGameActive = false;
            uiManager.ShowFailPanel();
        }
    }
}