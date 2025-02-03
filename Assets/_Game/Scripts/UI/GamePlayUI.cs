using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
namespace _Main._UI
{


    public class GameplayUI : MonoBehaviour
    {
        [Header("Text Elements")]
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private TextMeshProUGUI _scoreText;

        [Header("Buttons")]
        [SerializeField] private Button _settingsButton;

        [Header("Score Progress")]
        [SerializeField] private Slider _scoreSlider;
        [SerializeField] private RectTransform _sliderMarksContainer;
        [SerializeField] private GameObject _thresholdMarkPrefab;

        [Header("Visual Settings")]
        [SerializeField] private Color _normalMarkColor = Color.white;
        [SerializeField] private Color _reachedMarkColor = Color.green;
        [SerializeField] private float _markWidth = 4f;
        [SerializeField] private float _markHeight = 20f;

        private UIManager _uiManager;

        private int _targetScore;
        private int[] _thresholds;
        private Image[] _thresholdMarks;

        private void Awake()
        {
            _uiManager = GetComponentInParent<UIManager>();
            if (_settingsButton != null)
            {
                _settingsButton.onClick.AddListener(OnSettingsButtonClicked);
            }
        }

        public void Initialize(int levelNumber, int targetScore, int[] thresholds)
        {
            _targetScore = targetScore;
            _thresholds = thresholds;

            SetupUI(levelNumber);
            SetupScoreSlider();
            CreateThresholdMarks();
        }
        private void OnSettingsButtonClicked()
        {
            if (_uiManager != null)
            {
                _uiManager.ShowSettings();
            }
        }
        private void SetupUI(int levelNumber)
        {
            _levelText.text = $"Level {levelNumber}";
            _scoreText.text = $"0/{_targetScore}";
        }

        private void SetupScoreSlider()
        {
            _scoreSlider.minValue = 0;
            _scoreSlider.maxValue = _targetScore;
            _scoreSlider.value = 0;

            // Slider fill'in rengini ayarla
            Image fillImage = _scoreSlider.fillRect.GetComponent<Image>();
            fillImage.color = new Color(0.2f, 0.8f, 0.2f); // Ye?ilimsi bir renk
        }


        private void CreateThresholdMarks()
        {
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

                // Yüzdelik pozisyon hesapla
                float normalizedPosition = (float)_thresholds[i] / _targetScore * 100f;

                markRect.anchorMin = new Vector2(normalizedPosition / 100f, 0.5f);
                markRect.anchorMax = new Vector2(normalizedPosition / 100f, 0.5f);
                markRect.sizeDelta = new Vector2(_markWidth, _markHeight);
                markRect.anchoredPosition = Vector2.zero;

                _thresholdMarks[i] = markImage;
                markImage.color = _normalMarkColor;
            }
        }
        public void UpdateScore(int currentScore)
        {
            // Skoru güncelle
            if (_scoreText != null)
                _scoreText.text = $"{currentScore}/{_targetScore}";

            // Slider de?erini güncelle (do?rudan currentScore kullan)
            if (_scoreSlider != null)
            {
                _scoreSlider.value = currentScore;

                // Slider animasyonunu ba?lat
                _scoreSlider.DOValue(currentScore, 0.3f).SetEase(Ease.OutQuad);
            }

            // E?ik i?aretlerini güncelle
            UpdateThresholdMarks(currentScore);
        }

        private void UpdateThresholdMarks(int currentScore)
        {
            for (int i = 0; i < _thresholds.Length; i++)
            {
                if (_thresholdMarks[i] != null)
                {
                    if (currentScore >= _thresholds[i])
                    {
                        // ??aret geçildi?inde rengi de?i?tir ve efekt ekle
                        if (_thresholdMarks[i].color != _reachedMarkColor)
                        {
                            _thresholdMarks[i].DOColor(_reachedMarkColor, 0.3f);
                            PlayThresholdReachedEffect(_thresholdMarks[i].transform);
                        }
                    }
                }
            }
        }
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        private void PlayThresholdReachedEffect(Transform markTransform)
        {
            // Scale animasyonu
            markTransform.DOScale(Vector3.one * 1.5f, 0.2f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    markTransform.DOScale(Vector3.one, 0.2f)
                        .SetEase(Ease.InQuad);
                });
        }

        public void UpdateTimer(float remainingTime)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            _timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }
}