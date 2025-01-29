using UnityEngine;
using System.Collections.Generic;

namespace SerapKeremGameTools._Game._PopUpSystem
{
    /// <summary>
    /// This class is responsible for testing the pop-up text system.
    /// It listens for user input (mouse click or screen touch) and shows a pop-up with a random message
    /// from a list of options. The pop-up's position, duration, and animation type can be configured.
    /// </summary>
    public class PopupTextTest : MonoBehaviour
    {
        [Header("Pop-Up Settings")]
        [SerializeField, Tooltip("List of messages to choose from when creating a pop-up.")]
        private List<string> _popUpTextOptions = new List<string>();

        [SerializeField, Tooltip("The position at which the pop-up text will appear.")]
        private Vector3 _popUpPosition = Vector3.zero;

        [SerializeField, Range(0.1f, 2f), Tooltip("Duration for which the pop-up text will remain visible.")]
        private float _popUpDuration = 0.5f;

        [SerializeField, Tooltip("Animation type for the pop-up text.")]
        private PopUpAnimationType _animationType = PopUpAnimationType.ScaleAndFade;

        private System.Random _random = new System.Random();

        /// <summary>
        /// Update is called once per frame. This checks for user input and triggers the pop-up if needed.
        /// </summary>
        private void Update()
        {
            // Check for mouse click or screen touch to show the pop-up
            if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
            {
                ShowPopUp();
            }
        }

        /// <summary>
        /// This method selects a random message from the list of options and shows it as a pop-up.
        /// </summary>
        private void ShowPopUp()
        {
            if (_popUpTextOptions.Count == 0) return;

            Debug.Log("ShowPopUp");

            // Randomly choose a message from the available options
            string message = _popUpTextOptions[_random.Next(_popUpTextOptions.Count)];

            // Show the pop-up with the selected message, duration, and animation type
            PopUpTextManager.Instance.ShowPopUpText(_popUpPosition, message, _popUpDuration, _animationType);
        }
    }
}
