using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TriInspector;

namespace _Main._UI
{
    /// <summary>
    /// Manages the fail panel UI and provides functionality to restart the game.
    /// </summary>
    public class FailPanel : MonoBehaviour
    {
        [Group("UI Elements")]
        [PropertyTooltip("Button to restart the game.")]
        [SerializeField, Required] private Button _restartButton;

        /// <summary>
        /// Initializes the FailPanel by setting up the restart button click listener.
        /// </summary>
        private void Awake()
        {
            // Check if restart button exists, add listener if valid.
            _restartButton?.onClick.AddListener(OnRestartClicked);
        }

        /// <summary>
        /// Displays the fail panel with an animation.
        /// </summary>
        public void Show()
        {
            // Set the panel active and apply a scaling animation to show the panel
            gameObject.SetActive(true);
            transform.DOScale(Vector3.one, 0.3f)
                .From(Vector3.zero)
                .SetEase(Ease.OutBack);
        }

        /// <summary>
        /// Handles the restart button click event and reloads the current scene.
        /// </summary>
        private void OnRestartClicked()
        {
            // Reset the time scale to normal and reload the current scene
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
