using SerapKeremGameTools._Game._Singleton;
using UnityEngine;
using UnityEngine.Events;

namespace SerapKeremGameTools._Game._TimeSystem
{
    /// <summary>
    /// Manages game time and countdown functionality, providing events for key time states.
    /// </summary>
    public class TimeManager : MonoSingleton<TimeManager>
    {
        /// <summary>
        /// Total elapsed game time in seconds.
        /// </summary>
        [Tooltip("Total elapsed game time in seconds.")]
        private float gameTimeElapsed;

        /// <summary>
        /// Indicates whether the game time is currently running.
        /// </summary>
        [Tooltip("Indicates whether the game time is currently running.")]
        private bool isGameTimeRunning;

        /// <summary>
        /// Remaining time for the countdown in seconds.
        /// </summary>
        [Tooltip("Remaining time for the countdown in seconds.")]
        private float countdownTimeLeft;

        /// <summary>
        /// Indicates whether the countdown is active.
        /// </summary>
        [Tooltip("Indicates whether the countdown is active.")]
        private bool countdownActive;

        /// <summary>
        /// Event invoked when the game time starts.
        /// </summary>
        [Tooltip("Event invoked when the game time starts.")]
        public UnityEvent OnTimeStart = new UnityEvent();

        /// <summary>
        /// Event invoked when the game time ends.
        /// </summary>
        [Tooltip("Event invoked when the game time ends.")]
        public UnityEvent OnTimeEnd = new UnityEvent();

        /// <summary>
        /// Event invoked when a countdown starts.
        /// </summary>
        [Tooltip("Event invoked when a countdown starts.")]
        public UnityEvent OnCountDownStart = new UnityEvent();

        /// <summary>
        /// Event invoked when a countdown ends.
        /// </summary>
        [Tooltip("Event invoked when a countdown ends.")]
        public UnityEvent OnCountDownEnd = new UnityEvent();

        /// <summary>
        /// Initializes the singleton instance.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
        }

        private void Update()
        {
            // Updates the game time if it is running
            if (isGameTimeRunning)
                gameTimeElapsed += Time.deltaTime;

            // Updates the countdown if it is active
            if (countdownActive)
            {
                countdownTimeLeft -= Time.deltaTime;

                if (countdownTimeLeft <= 0)
                {
                    countdownActive = false;
                    countdownTimeLeft = 0;

                    // Invokes the countdown end event
                    OnCountDownEnd.Invoke();

                    // Automatically starts game time after countdown ends
                    StartTime();
                }
            }
        }

        /// <summary>
        /// Starts tracking game time from zero and invokes the time start event.
        /// </summary>
        public void StartTime()
        {
            isGameTimeRunning = true;
            gameTimeElapsed = 0;
            OnTimeStart.Invoke();
        }

        /// <summary>
        /// Pauses the game time, halting the elapsed time tracking.
        /// </summary>
        public void PauseTime()
        {
            isGameTimeRunning = false;
        }

        /// <summary>
        /// Resumes the game time, continuing elapsed time tracking.
        /// </summary>
        public void ResumeTime()
        {
            isGameTimeRunning = true;
        }

        /// <summary>
        /// Starts a countdown from the specified duration and invokes the countdown start event.
        /// </summary>
        /// <param name="duration">Duration of the countdown in seconds.</param>
        public void StartCountdown(float duration)
        {
            countdownTimeLeft = duration;
            countdownActive = true;
            OnCountDownStart.Invoke();
        }

        /// <summary>
        /// Gets the total game time elapsed since the last start.
        /// </summary>
        /// <returns>Total elapsed game time in seconds.</returns>
        public float GetGameTimeElapsed()
        {
            return gameTimeElapsed;
        }

        /// <summary>
        /// Checks whether a countdown is currently active.
        /// </summary>
        /// <returns>True if a countdown is active, otherwise false.</returns>
        public bool IsCountdownActive()
        {
            return countdownActive;
        }

        /// <summary>
        /// Gets the remaining time left in the active countdown.
        /// </summary>
        /// <returns>Time left in seconds for the countdown.</returns>
        public float GetCountdownTimeLeft()
        {
            return countdownTimeLeft;
        }
    }
}
