using UnityEngine;

namespace SerapKeremGameTools._Game._PopUpSystem
{
    /// <summary>
    /// Defines the types of animations for pop-up text.
    /// This enum is used to specify different animation types for pop-up texts.
    /// </summary>
    public enum PopUpAnimationType
    {
        /// <summary>
        /// Scale and fade animation.
        /// The pop-up text starts from zero size, scales up to its full size, and then fades out.
        /// </summary>
        [Tooltip("Scale and fade animation. The text starts small, scales up, then fades out.")]
        ScaleAndFade, 

        /// <summary>
        /// Slide up animation.
        /// The pop-up text moves upward from its initial position.
        /// </summary>
        [Tooltip("Slide up animation. The text moves upwards from its starting position.")]
        SlideUp,    

        /// <summary>
        /// Slide down animation.
        /// The pop-up text moves downward from its initial position.
        /// </summary>
        [Tooltip("Slide down animation. The text moves downwards from its starting position.")]
        SlideDown,   

        /// <summary>
        /// Bounce animation.
        /// The pop-up text bounces up and down a few times.
        /// </summary>
        [Tooltip("Bounce animation. The text bounces up and down multiple times.")]
        Bounce         
    }
}
