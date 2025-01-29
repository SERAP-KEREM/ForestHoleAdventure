using UnityEngine;
using System.Collections.Generic;

namespace SerapKeremGameTools._Game._PopUpSystem
{
    /// <summary>
    /// This class is responsible for testing the pop-up sprite system.
    /// It listens for user input (mouse click or screen touch) and shows a pop-up sprite
    /// from a list of sprite options. The pop-up's position, duration, and animation type can be configured.
    /// </summary>
    public class PopupIconTest : MonoBehaviour
    {
        [Header("Pop-Up Sprite Settings")]
        [SerializeField, Tooltip("List of sprites to choose from when creating a pop-up.")]
        private List<Sprite> popUpSpriteOptions = new List<Sprite>();

        [SerializeField, Tooltip("The position at which the pop-up sprite will appear.")]
        private Vector3 popUpPosition = Vector3.zero;

        [SerializeField, Range(0.1f, 2f), Tooltip("Duration for which the pop-up sprite will remain visible.")]
        private float popUpDuration = 0.5f;

        [SerializeField, Tooltip("Animation type for the pop-up sprite.")]
        private PopUpAnimationType animationType = PopUpAnimationType.ScaleAndFade;

        private System.Random random = new System.Random();

        /// <summary>
        /// Update is called once per frame. This checks for user input and triggers the pop-up if needed.
        /// </summary>
        private void Update()
        {
            // Check for mouse click or screen touch to show the pop-up sprite
            if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
            {
                ShowPopUp();
            }
        }

        /// <summary>
        /// This method selects a random sprite from the list of options and shows it as a pop-up.
        /// </summary>
        private void ShowPopUp()
        {
            if (popUpSpriteOptions.Count == 0) return;

            // Randomly choose a sprite from the available options
            Sprite sprite = popUpSpriteOptions[random.Next(popUpSpriteOptions.Count)];

            // Show the pop-up with the selected sprite, duration, and animation type
            PopUpIconManager.Instance.ShowPopUpIcon(popUpPosition, sprite, popUpDuration, animationType);
        }
    }
}
