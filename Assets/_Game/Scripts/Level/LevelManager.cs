using UnityEngine;
using TMPro;
using UnityEngine.Events;
using _Main.Hole;

namespace _Main._Level
{
    public class LevelManager : MonoBehaviour
    {
        [Header("Level Settings")]
        [SerializeField] private LevelData currentLevel;
        [SerializeField] private HoleManager holeManager;

        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI levelText;

        public UnityEvent onLevelComplete;
        public UnityEvent onLevelFailed;

        private int currentScore;
        private float remainingTime;
        private int currentThresholdIndex;
        private bool isGameActive;

        private void Start()
        {
            InitializeLevel();
        }

        private void InitializeLevel()
        {
            currentScore = 0;
            remainingTime = currentLevel.LevelTime;
            currentThresholdIndex = 0;
            isGameActive = true;

            // Ba?lang?ç hole boyutunu ayarla
            holeManager.SetHoleSize(currentLevel.InitialHoleSize);

            UpdateUI();
            levelText.text = $"Level {currentLevel.LevelNumber}";
        }

        private void Update()
        {
            if (!isGameActive) return;
            UpdateTimer();
            UpdateUI();
        }

        public void AddScore(int points)
        {
            if (!isGameActive) return;

            currentScore += points;
            CheckGrowthThreshold();
            UpdateUI();

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
            if (remainingTime <= 0)
            {
                remainingTime = 0;
                LevelFailed();
            }
        }

        private void UpdateUI()
        {
            scoreText.text = $"Score: {currentScore}/{currentLevel.TargetScore}";

            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }

        private void LevelComplete()
        {
            isGameActive = false;
            onLevelComplete?.Invoke();
            Debug.Log("Level Complete!");
        }

        private void LevelFailed()
        {
            isGameActive = false;
            onLevelFailed?.Invoke();
            Debug.Log("Level Failed!");
        }
    }
}