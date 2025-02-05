using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using SerapKeremGameTools._Game._AudioSystem;
using TMPro;
using UnityEngine.SceneManagement;
using SerapKeremGameTools._Game._SaveLoadSystem;
using TriInspector;

namespace _Main._UI
{
    /// <summary>
    /// Manages the settings panel UI elements and audio volume settings.
    /// </summary>
    [DeclareFoldoutGroup("UI Elements", Title = "UI Elements")]
    [DeclareFoldoutGroup("Audio", Title = "Audio")]
    public class SettingsPanel : MonoBehaviour
    {
        
        [SerializeField, Group("UI Elements"), PropertyTooltip("Close button to hide the settings panel.")]
        private Button _closeButton;

        [SerializeField, Group("UI Elements"), PropertyTooltip("New game button to restart the game.")] 
        private Button _newGameButton;

        [SerializeField, Group("UI Elements"), PropertyTooltip("Resume button to resume the game.")]
        private Button _resumeButton;

        [SerializeField, Group("UI Elements"), PropertyTooltip("Slider to control music volume.")] 
        private Slider _musicSlider;

        [SerializeField, Group("UI Elements"), PropertyTooltip("Text displaying the music volume percentage.")]
        private TextMeshProUGUI _musicValueText;

        [SerializeField, Group("UI Elements"), PropertyTooltip("Canvas group to control UI transparency.")] 
        private CanvasGroup _canvasGroup;

        [Group("Audio")]
        [SerializeField, PropertyTooltip("Background music audio clip name.")] private string _backgroundMusicName = "BackgroundMusic";
        private const string MUSIC_VOLUME_KEY = "MusicVolume";


        private void Awake()
        {

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

            SetupButtons();
            LoadSavedVolume();
        }

        /// <summary>
        /// Sets up button listeners.
        /// </summary>
        private void SetupButtons()
        {
            if (_closeButton != null) _closeButton.onClick.AddListener(OnResumeClicked);
            if (_newGameButton != null) _newGameButton.onClick.AddListener(OnNewGameClicked);
            if (_resumeButton != null) _resumeButton.onClick.AddListener(OnResumeClicked);
            if (_musicSlider != null) _musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }

        /// <summary>
        /// Loads the saved music volume setting.
        /// </summary>
        private void LoadSavedVolume()
        {
            float savedVolume = LoadManager.LoadData<float>(MUSIC_VOLUME_KEY, 1f);
            if (_musicSlider != null)
            {
                _musicSlider.value = savedVolume;
                UpdateMusicValueText(savedVolume);
                ApplyVolumeToAudioSources(savedVolume);
            }
        }

        /// <summary>
        /// Closes the settings panel and resumes the game.
        /// </summary>
        private void OnCloseButtonClicked()
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.HideSettings();
            }
        }

        /// <summary>
        /// Restarts the game by reloading the current scene.
        /// </summary>
        private void OnNewGameClicked()
        {
            DOTween.KillAll();
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        /// <summary>
        /// Resumes the game and hides the settings panel.
        /// </summary>
        private void OnResumeClicked()
        {
            Time.timeScale = 1f;
            UIManager.Instance.HideSettings();
        }

        /// <summary>
        /// Displays the settings panel with animations.
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
            DOVirtual.DelayedCall(0.1f, () => Time.timeScale = 0f).SetUpdate(true);

            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = 0f;
                _canvasGroup.DOFade(1f, 0.3f).SetEase(Ease.OutQuad).SetUpdate(true);
                transform.DOScale(Vector3.one, 0.3f)
                    .From(Vector3.one * 0.8f)
                    .SetEase(Ease.OutBack)
                    .SetUpdate(true);
            }
        }

        /// <summary>
        /// Hides the settings panel with animations.
        /// </summary>
        public void Hide()
        {
            if (_canvasGroup != null)
            {
                _canvasGroup.DOFade(0f, 0.2f)
                    .SetEase(Ease.InQuad)
                    .SetUpdate(true)
                    .OnComplete(() => {
                        gameObject.SetActive(false);
                        Time.timeScale = 1f;
                    });

                transform.DOScale(Vector3.one * 0.8f, 0.2f)
                    .SetEase(Ease.InQuad)
                    .SetUpdate(true);
            }
            else
            {
                gameObject.SetActive(false);
                Time.timeScale = 1f;
            }
        }

        /// <summary>
        /// Updates the music volume when the slider value is changed.
        /// </summary>
        private void OnMusicVolumeChanged(float value)
        {
            SaveManager.SaveData(MUSIC_VOLUME_KEY, value);
            UpdateMusicValueText(value);
            ApplyVolumeToAudioSources(value);

            if (value > 0 && AudioManager.Instance != null && !AudioManager.Instance.IsPlaying(_backgroundMusicName))
            {
                AudioManager.Instance.PlayAudio(_backgroundMusicName);
            }
        }

        /// <summary>
        /// Updates the music volume percentage text.
        /// </summary>
        private void UpdateMusicValueText(float value)
        {
            if (_musicValueText != null)
            {
                _musicValueText.text = $"{(value * 100):F0}%";
            }
        }

        /// <summary>
        /// Applies the volume setting to all audio sources in the scene.
        /// </summary>
        private void ApplyVolumeToAudioSources(float volume)
        {
            AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
            foreach (var source in audioSources)
            {
                source.volume = volume;
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
