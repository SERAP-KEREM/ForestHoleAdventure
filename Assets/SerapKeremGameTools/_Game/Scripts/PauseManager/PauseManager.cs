using SerapKeremGameTools._Game._Singleton;
using SerapKeremGameTools._Game._TimeSystem;
using UnityEngine;
using UnityEngine.Events;

namespace SerapKeremGameTools._Game._PauseSystem
{
    /// <summary>
    /// Manages pausing and resuming the game, integrating with other systems such as TimeManager.
    /// </summary>
    public class PauseManager : MonoSingleton<PauseManager>
    {
        /// <summary>
        /// Indicates whether the game is currently paused.
        /// </summary>
        private bool isPaused;

        /// <summary>
        /// Event invoked when the game is paused or resumed.
        /// The bool parameter indicates the pause state (true = paused, false = resumed).
        /// </summary>
        public UnityEvent<bool> OnPauseStateChanged = new UnityEvent<bool>();

        /// <summary>
        /// Pauses the game, including time and other systems.
        /// </summary>
        public void PauseGame()
        {
            if (isPaused) return;

            isPaused = true;

            // Pause the game time
            TimeManager.Instance.PauseTime();

            // Trigger pause state event
            OnPauseStateChanged.Invoke(isPaused);

            // Optionally show the pause UI
            ShowPauseMenu();
        }

        /// <summary>
        /// Resumes the game from a paused state.
        /// </summary>
        public void ResumeGame()
        {
            if (!isPaused) return;

            isPaused = false;

            // Resume the game time
            TimeManager.Instance.ResumeTime();

            // Trigger pause state event
            OnPauseStateChanged.Invoke(isPaused);

            // Optionally hide the pause UI
            HidePauseMenu();
        }

        /// <summary>
        /// Toggles the pause state of the game.
        /// </summary>
        public void TogglePause()
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }

        /// <summary>
        /// Displays the pause menu UI.
        /// </summary>
        private void ShowPauseMenu()
        {
#if UNITY_EDITOR
            Debug.Log("Pause menu shown.");
#endif
            // Implement pause menu activation here
        }

        /// <summary>
        /// Hides the pause menu UI.
        /// </summary>
        private void HidePauseMenu()
        {
#if UNITY_EDITOR
            Debug.Log("Pause menu hidden.");
#endif
            // Implement pause menu deactivation here
        }

        /// <summary>
        /// Checks whether the game is currently paused.
        /// </summary>
        /// <returns>True if the game is paused, otherwise false.</returns>
        public bool IsGamePaused()
        {
            return isPaused;
        }

        /// <summary>
        /// Updates the game based on user input for pausing or resuming the game.
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) // You can change the key here
            {
                TogglePause();
            }
        }
    }
}
