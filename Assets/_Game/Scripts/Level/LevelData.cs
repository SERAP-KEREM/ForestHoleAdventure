using UnityEngine;
namespace _Main._Level
{

    [CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Level Settings")]
    [SerializeField] private int _levelNumber;
    [SerializeField] private int _targetScore;
    [SerializeField] private float _levelTime = 180f; // 3 dakika

    [Header("Hole Settings")]
    [SerializeField] private float _initialHoleSize = 1f;
    [SerializeField] private float _maxHoleSize = 10f;
    [SerializeField] private float _baseGrowthFactor = 0.001f;
    [SerializeField] private int _scoreThresholdForGrowth = 500; // Bu puandan sonra büyüme h?zlan?r
        [SerializeField] private float _growthAmount = 0.3f;
        [Header("Score Thresholds for Growth")]
        [SerializeField] private int[] _scoreThresholds = { 100, 250, 400, 600, 800 };


        [Header("Spawn Settings")]
    [SerializeField] private float _respawnInterval = 5f;
    [SerializeField] private int _minActiveObjects = 10;

        // Getters
        // Public properties
        public int LevelNumber => _levelNumber;
        public int TargetScore => _targetScore;
        public float LevelTime => _levelTime;
        public float InitialHoleSize => _initialHoleSize;
        public float GrowthAmount => _growthAmount;
        public int[] ScoreThresholds => _scoreThresholds;
    public float MaxHoleSize => _maxHoleSize;
    public float BaseGrowthFactor => _baseGrowthFactor;
    public int ScoreThresholdForGrowth => _scoreThresholdForGrowth;
    public float RespawnInterval => _respawnInterval;
    public int MinActiveObjects => _minActiveObjects;
}
}