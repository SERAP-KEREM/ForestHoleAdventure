using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using TriInspector;
using _Main._Managers;

namespace _Main._UI
{
    /// <summary>
    /// Manages the gameplay UI elements like score, level, timer, and score progress.
    /// Provides functionalities to update score, timer, and display threshold marks.
    /// </summary>
    public class GameplayUI : MonoBehaviour
    {
        [Header("Text Elements")]
        [Tooltip("Text displaying the current level number.")]
        [SerializeField, Required] private TextMeshProUGUI _levelText;

        [Tooltip("Text displaying the remaining time.")]
        [SerializeField, Required] private TextMeshProUGUI _timerText;

        [Tooltip("Text displaying the current score and target score.")]
        [SerializeField, Required] private TextMeshProUGUI _scoreText;

        [Header("Buttons")]
        [Tooltip("Button to open the settings panel.")]
        [SerializeField, Required] private Button _settingsButton;

        [Header("Score Progress")]
        [Tooltip("Slider showing the current score progress.")]
        [SerializeField, Required] private Slider _scoreSlider;

        [Tooltip("Container for score threshold marks.")]
        [SerializeField, Required] private RectTransform _sliderMarksContainer;

        [Tooltip("Prefab for threshold marks.")]
        [SerializeField, Required] private GameObject _thresholdMarkPrefab;

        [Header("Visual Settings")]
        [Tooltip("Color of the threshold marks when not reached.")]
        [SerializeField] private Color _normalMarkColor = Color.white;

        [Tooltip("Color of the threshold marks when reached.")]
        [SerializeField] private Color _reachedMarkColor = Color.green;

        [Tooltip("Width of the threshold marks.")]
        [SerializeField] private float _markWidth = 4f;

        [Tooltip("Height of the threshold marks.")]
        [SerializeField] private float _markHeight = 20f;

        private UIManager _uiManager;
        private int _targetScore;
        private int[] _thresholds;
        private Image[] _thresholdMarks;

        /// <summary>
        /// Initializes references and button interactions.
        /// </summary>
        private void Awake()
        {
           _uiManager=GameManager.Instance.GetUIManager();
            SetupSettingsButton();
            ValidateReferences();
        }

        /// <summary>
        /// Validates the essential UI references.
        /// </summary>
        private void ValidateReferences()
        {
            if (_levelText == null) Debug.LogError("Level Text is missing!");
            if (_scoreText == null) Debug.LogError("Score Text is missing!");
            if (_timerText == null) Debug.LogError("Timer Text is missing!");
            if (_scoreSlider == null) Debug.LogError("Score Slider is missing!");
            if (_settingsButton == null) Debug.LogError("Settings Button is missing!");
        }

        /// <summary>
        /// Sets up the settings button click listener.
        /// </summary>
        private void SetupSettingsButton()
        {
            if (_settingsButton != null)
            {
                _settingsButton.onClick.RemoveAllListeners();
                _settingsButton.onClick.AddListener(() => _uiManager.ShowSettings());
            }
        }

        /// <summary>
        /// Initializes the gameplay UI with level, target score, and thresholds.
        /// </summary>
        /// <param name="levelNumber">The level number.</param>
        /// <param name="targetScore">The target score for this level.</param>
        /// <param name="thresholds">The score thresholds to track.</param>
        public void Initialize(int levelNumber, int targetScore, int[] thresholds)
        {
            Debug.Log($"Initializing GameplayUI: Level {levelNumber}, Target {targetScore}");

            _targetScore = targetScore;
            _thresholds = thresholds;

            SetupUI(levelNumber);
            SetupScoreSlider();
            CreateThresholdMarks();
        }

        /// <summary>
        /// Sets up the UI elements with initial values.
        /// </summary>
        private void SetupUI(int levelNumber)
        {
            if (_levelText != null)
            {
                _levelText.text = $"LEVEL {levelNumber}";
            }

            if (_scoreText != null)
            {
                _scoreText.text = $"0/{_targetScore}";
            }

            if (_timerText != null)
            {
                _timerText.text = "00:00";
            }
        }

        /// <summary>
        /// Configures the score slider based on the target score.
        /// </summary>
        private void SetupScoreSlider()
        {
            _scoreSlider.minValue = 0;
            _scoreSlider.maxValue = _targetScore;
            _scoreSlider.value = 0;
            _scoreSlider.interactable = false;

            Image fillImage = _scoreSlider.fillRect.GetComponent<Image>();
            fillImage.color = new Color(0.2f, 0.8f, 0.2f);
        }

        /// <summary>
        /// Creates marks on the score slider based on the score thresholds.
        /// </summary>
        private void CreateThresholdMarks()
        {
            // Clear existing marks
            foreach (Transform child in _sliderMarksContainer)
            {
                Destroy(child.gameObject);
            }

            _thresholdMarks = new Image[_thresholds.Length];

            for (int i = 0; i < _thresholds.Length; i++)
            {
                GameObject markObj = Instantiate(_thresholdMarkPrefab, _sliderMarksContainer);
                RectTransform markRect = markObj.GetComponent<RectTransform>();
                Image markImage = markObj.GetComponent<Image>();

                float normalizedPosition = (float)_thresholds[i] / _targetScore * 100f;

                markRect.anchorMin = new Vector2(normalizedPosition / 100f, 0.5f);
                markRect.anchorMax = new Vector2(normalizedPosition / 100f, 0.5f);
                markRect.sizeDelta = new Vector2(_markWidth, _markHeight);
                markRect.anchoredPosition = Vector2.zero;

                _thresholdMarks[i] = markImage;
                markImage.color = _normalMarkColor;
            }
        }

        /// <summary>
        /// Updates the score display and progress.
        /// </summary>
        public void UpdateScore(int currentScore)
        {
            if (_scoreText != null)
                _scoreText.text = $"{currentScore}/{_targetScore}";

            if (_scoreSlider != null)
            {
                _scoreSlider.value = currentScore;
                _scoreSlider.DOValue(currentScore, 0.3f).SetEase(Ease.OutQuad);
            }

            UpdateThresholdMarks(currentScore);
        }

        /// <summary>
        /// Updates the color of the threshold marks based on the current score.
        /// </summary>
        private void UpdateThresholdMarks(int currentScore)
        {
            for (int i = 0; i < _thresholds.Length; i++)
            {
                if (_thresholdMarks[i] != null)
                {
                    if (currentScore >= _thresholds[i])
                    {
                        if (_thresholdMarks[i].color != _reachedMarkColor)
                        {
                            _thresholdMarks[i].DOColor(_reachedMarkColor, 0.3f);
                            PlayThresholdReachedEffect(_thresholdMarks[i].transform);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Displays the gameplay UI.
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Plays an animation when a threshold is reached.
        /// </summary>
        private void PlayThresholdReachedEffect(Transform markTransform)
        {
            markTransform.DOScale(Vector3.one * 1.5f, 0.2f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    markTransform.DOScale(Vector3.one, 0.2f)
                        .SetEase(Ease.InQuad);
                });
        }

        /// <summary>
        /// Updates the timer display with the remaining time.
        /// </summary>
        public void UpdateTimer(float remainingTime)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            _timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }
}
