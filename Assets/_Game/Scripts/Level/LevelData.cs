using UnityEngine;
namespace _Main._Level
{

    [CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
    public class LevelData : ScriptableObject
    {
        [Header("Level Settings")]
        [SerializeField] private int _levelNumber;
        [SerializeField] private int _targetScore;
        [SerializeField] private float _levelTime = 180f;

        [Header("Hole Settings")]
        [SerializeField] private float _initialHoleSize = 1f;
        [SerializeField] private float _maxHoleSize = 10f;
        [SerializeField] private float _baseGrowthFactor = 0.001f;
        [SerializeField] private int _scoreThresholdForGrowth = 500; 
        [SerializeField] private float _growthAmount = 0.3f;
        [Header("Score Thresholds for Growth")]
        [SerializeField] private int[] _scoreThresholds = { 100, 250, 400, 600, 800 };

        // Getters
        // Public properties
        public int LevelNumber => _levelNumber;
        public int TargetScore => _targetScore;
        public float LevelTime => _levelTime;
        public float InitialHoleSize => _initialHoleSize;
        public float GrowthAmount => _growthAmount;
        public int[] ScoreThresholds => _scoreThresholds;

    }
}