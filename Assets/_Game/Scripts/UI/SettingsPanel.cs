using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using SerapKeremGameTools._Game._AudioSystem;
using TMPro;
using _Main.Managers;

namespace _Main._UI
{
    public class SettingsPanel : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private Button _closeButton;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private TextMeshProUGUI _musicValueText;
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Audio")]
        [SerializeField] private string _backgroundMusicName = "BackgroundMusic";

        private UIManager _uiManager;
        private AudioManager _audioManager;

        private void Awake()
        {
            _uiManager = GetComponentInParent<UIManager>();
            _audioManager = AudioManager.Instance;

            if (_closeButton != null)
            {
                _closeButton.onClick.AddListener(OnCloseButtonClicked);
            }

            if (_canvasGroup == null)
            {
                _canvasGroup = GetComponent<CanvasGroup>();
            }

            if (_musicSlider != null)
            {
                _musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            }

            LoadSavedVolume();
        }

        private void LoadSavedVolume()
        {
            if (_musicSlider != null)
            {
                float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 100f); // Varsay?lan de?er 100
                _musicSlider.value = savedVolume;
                _musicValueText.text = $"{Mathf.RoundToInt(savedVolume)}"; // Yuvarlanm?? gösterim
                OnMusicVolumeChanged(savedVolume);
            }
        }

        private void OnCloseButtonClicked()
        {
            if (_uiManager != null)
            {
                _uiManager.HideSettings();
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);

            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = 0f;
                _canvasGroup.DOFade(1f, 0.3f).SetEase(Ease.OutQuad);
                transform.DOScale(Vector3.one, 0.3f).From(Vector3.one * 0.8f).SetEase(Ease.OutBack);
            }
        }

        public void Hide()
        {
            if (_canvasGroup != null)
            {
                _canvasGroup.DOFade(0f, 0.2f).SetEase(Ease.InQuad)
                    .OnComplete(() => gameObject.SetActive(false));
                transform.DOScale(Vector3.one * 0.8f, 0.2f).SetEase(Ease.InQuad);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void OnMusicVolumeChanged(float value)
        {
            // 0-100 aras?ndaki de?eri 0-1'e dönü?tür
            float normalizedValue = value / 100f;

            // Ses seviyesini kaydet
            PlayerPrefs.SetFloat("MusicVolume", value);
            PlayerPrefs.Save();

            // Ses seviyesi text'ini güncelle
            _musicValueText.text = $"{Mathf.RoundToInt(value)}";

            // Tüm ses kaynaklar?n?n ses seviyesini güncelle
            AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
            foreach (var source in audioSources)
            {
                source.volume = normalizedValue;
            }

            // E?er ses kapal?ysa ve slider de?eri art?r?ld?ysa müzi?i tekrar ba?lat
            if (normalizedValue > 0 && !AudioManager.Instance.IsPlaying(_backgroundMusicName))
            {
                GameManager.Instance.PlayBackgroundMusic();
            }
        }

        private void OnDestroy()
        {
            if (_musicSlider != null)
            {
                _musicSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
            }
        }
    }
}
