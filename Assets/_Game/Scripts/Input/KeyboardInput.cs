using UnityEngine;

namespace _Main._InputSystem
{
    /// <summary>
    /// Handles keyboard input for horizontal and vertical movement axes.
    /// Supports both WASD and arrow keys for input.
    /// </summary>
    public class KeyboardInput : MonoBehaviour
    {
        #region Properties

        /// <summary>
        /// Gets the horizontal input value (-1, 0, 1).
        /// </summary>
        public float Horizontal { get; private set; }

        /// <summary>
        /// Gets the vertical input value (-1, 0, 1).
        /// </summary>
        public float Vertical { get; private set; }

        #endregion

        #region Update

        private void Update()
        {
            // Reset the inputs to 0 before checking key presses
            Horizontal = 0;
            Vertical = 0;

            // Check vertical movement (W, S, UpArrow, DownArrow)
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) Vertical = 1;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) Vertical = -1;

            // Check horizontal movement (A, D, LeftArrow, RightArrow)
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) Horizontal = -1;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) Horizontal = 1;
        }

        #endregion
    }
}
