using UnityEngine;

namespace _Main._Level
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
    public class LevelData : ScriptableObject
    {
        #region Serialized Fields

        [Header("Level Settings")]
        [Tooltip("The unique number for the level.")]
        [SerializeField] private int _levelNumber;

        [Tooltip("The score required to complete the level.")]
        [SerializeField] private int _targetScore;

        [Tooltip("Time limit for the level.")]
        [SerializeField] private float _levelTime = 180f;

        [Tooltip("Prefab for generating the level.")]
        [SerializeField] private LevelGenerator _levelGeneratorPrefab;

        [Header("Hole Settings")]
        [Tooltip("Initial size of the hole.")]
        [SerializeField] private float _initialHoleSize = 1f;

        [Tooltip("Maximum size the hole can reach.")]
        [SerializeField] private float _maxHoleSize = 10f;

        [Tooltip("Base growth factor for the hole size.")]
        [SerializeField] private float _baseGrowthFactor = 0.001f;

        [Tooltip("Score threshold required to trigger growth.")]
        [SerializeField] private int _scoreThresholdForGrowth = 500;

        [Tooltip("Amount the hole grows per threshold.")]
        [SerializeField] private float _growthAmount = 0.3f;

        [Header("Score Thresholds for Growth")]
        [Tooltip("Score thresholds at which the hole will grow.")]
        [SerializeField] private int[] _scoreThresholds = { 100, 250, 400, 600, 800 };

        #endregion

        #region Public Properties

        public int LevelNumber => _levelNumber;
        public int TargetScore => _targetScore;
        public float LevelTime => _levelTime;
        public float InitialHoleSize => _initialHoleSize;
        public float MaxHoleSize => _maxHoleSize;
        public float GrowthAmount => _growthAmount;
        public int[] ScoreThresholds => _scoreThresholds;
        public LevelGenerator LevelGeneratorPrefab => _levelGeneratorPrefab;

        #endregion
    }
}
