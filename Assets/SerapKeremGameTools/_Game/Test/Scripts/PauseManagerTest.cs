using SerapKeremGameTools._Game._TimeSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SerapKeremGameTools._Game._AudioSystem;
using SerapKeremGameTools._Game._ParticleEffectSystem;

namespace SerapKeremGameTools._Game._PauseSystem
{
    /// <summary>
    /// Manages the pause functionality, countdown, and game timer. 
    /// Controls audio and particle effects when the game is paused or resumed.
    /// </summary>
    public class PauseManagerTest : MonoBehaviour
    {
        [Header("UI Elements")]
        [Tooltip("Text element for displaying the countdown timer.")]
        public TextMeshProUGUI countdownText; // Countdown UI element
        [Tooltip("Text element for displaying the in-game timer.")]
        public TextMeshProUGUI timerText; // Timer UI element for game time
        [Tooltip("Button to trigger the pause functionality.")]
        public Button pauseButton; // Pause button to trigger pause and resume actions

        [Header("Audio Settings")]
        [SerializeField, Tooltip("Array of audio clips to play when the game starts.")]
        private string[] audioName;

        [Header("Particle Effect Settings")]
        [SerializeField, Tooltip("Array of particle effect names to play when the game starts.")]
        private string[] particleEffectName;

        private bool gameStarted; // Flag to check if the game has started
        [SerializeField, Tooltip("Initial countdown duration in seconds.")]
        private int countDown; // Countdown duration before the game starts

        private void Start()
        {
            // Set up UI elements and systems at the start
            countdownText.gameObject.SetActive(true);
            timerText.gameObject.SetActive(false);

            // Add listener for pause button click
            pauseButton.onClick.AddListener(OnPauseButtonClicked);

            // Start the countdown timer
            StartCountdown(countDown);
        }

        private void Update()
        {
            // If the game has started, update the timer and handle effects
            if (gameStarted)
            {
                HandleAudioAndParticleEffects();
                UpdateTimerText();
            }
        }

        /// <summary>
        /// Plays audio and particle effects if not already playing.
        /// </summary>
        private void HandleAudioAndParticleEffects()
        {
            if (audioName.Length > 0 && !AudioManager.Instance.IsPlaying(audioName[0]))
            {
                AudioManager.Instance.PlayAudio(audioName[0]);
                PlayParticleEffect();
            }
        }

        /// <summary>
        /// Plays the first particle effect from the list at the current position and rotation.
        /// </summary>
        private void PlayParticleEffect()
        {
            if (particleEffectName.Length > 0)
            {
                ParticleEffectManager.Instance.PlayParticle(particleEffectName[0], transform.position, transform.rotation);
            }
        }

        /// <summary>
        /// Updates the game time text on the screen.
        /// </summary>
        private void UpdateTimerText()
        {
            float elapsedTime = TimeManager.Instance.GetGameTimeElapsed();
            timerText.text = $"Time: {elapsedTime:F2}s";
        }

        /// <summary>
        /// Starts the countdown timer and triggers the countdown routine.
        /// </summary>
        /// <param name="duration">The countdown duration in seconds.</param>
        private void StartCountdown(float duration)
        {
            countDown = Mathf.CeilToInt(duration);
            StartCoroutine(CountdownRoutine());
        }

        /// <summary>
        /// Coroutine to handle the countdown logic.
        /// </summary>
        private IEnumerator CountdownRoutine()
        {
            while (countDown > 0)
            {
                countdownText.text = countDown.ToString();
                yield return new WaitForSeconds(1f);
                countDown--;
            }

            // Update UI after countdown ends
            countdownText.gameObject.SetActive(false);
            timerText.gameObject.SetActive(true);
            gameStarted = true;
            TimeManager.Instance.StartTime();
        }

        /// <summary>
        /// Handles the pause button click event to toggle pause and resume the game.
        /// </summary>
        private void OnPauseButtonClicked()
        {
            PauseManager.Instance.TogglePause();

            if (PauseManager.Instance.IsGamePaused())
            {
                // Pause the game, stop audio and particle effects
                AudioManager.Instance.PauseAllAudio();
                if (particleEffectName.Length > 0)
                {
                    ParticleEffectManager.Instance.StopAllEffects();
                }

#if UNITY_EDITOR
                Debug.Log("Game Paused");
#endif
            }
            else
            {
                // Resume the game, play audio and particle effects
                AudioManager.Instance.ResumeAllAudio();
                PlayParticleEffect();

#if UNITY_EDITOR
                Debug.Log("Game Resumed");
#endif
            }
        }
    }
}
