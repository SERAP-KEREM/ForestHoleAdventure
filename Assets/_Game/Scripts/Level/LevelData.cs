using UnityEngine;
using TriInspector;

namespace _Main._Level
{
    [DeclareFoldoutGroup("Level Settings", Title = "Level Settings")]
    [DeclareFoldoutGroup("Hole Settings", Title = "Hole Settings")]
    [DeclareFoldoutGroup("Score Thresholds for Growth", Title = "Score Thresholds")]
    [CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
    public class LevelData : ScriptableObject
    {
        #region Serialized Fields

        [Group("Level Settings")]
        [PropertyTooltip("The unique number for the level.")]
        [SerializeField] private int _levelNumber;

        [Group("Level Settings")]
        [PropertyTooltip("The score required to complete the level.")]
        [SerializeField] private int _targetScore;

        [Group("Level Settings")]
        [PropertyTooltip("Time limit for the level.")]
        [SerializeField] private float _levelTime = 180f;

        [Group("Level Settings")]
        [PropertyTooltip("Prefab for generating the level.")]
        [SerializeField] private LevelGenerator _levelGeneratorPrefab;

        [Group("Hole Settings")]
        [PropertyTooltip("Initial size of the hole.")]
        [SerializeField] private float _initialHoleSize = 1f;

        [Group("Hole Settings")]
        [PropertyTooltip("Maximum size the hole can reach.")]
        [SerializeField] private float _maxHoleSize = 10f;

        [Group("Hole Settings")]
        [PropertyTooltip("Base growth factor for the hole size.")]
        [SerializeField] private float _baseGrowthFactor = 0.001f;

        [Group("Hole Settings")]
        [PropertyTooltip("Score threshold required to trigger growth.")]
        [SerializeField] private int _scoreThresholdForGrowth = 500;

        [Group("Hole Settings")]
        [PropertyTooltip("Amount the hole grows per threshold.")]
        [SerializeField] private float _growthAmount = 0.3f;

        [Group("Score Thresholds for Growth")]
        [PropertyTooltip("Score thresholds at which the hole will grow.")]
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
