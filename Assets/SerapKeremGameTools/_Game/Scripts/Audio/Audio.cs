using UnityEngine;

namespace SerapKeremGameTools._Game._AudioSystem
{
    /// <summary>
    /// Contains all the properties related to an audio clip.
    /// </summary>
    [System.Serializable]
    public class Audio
    {
        [Tooltip("Unique name for the audio. This name will be used in PlayAudio.")]
        [SerializeField]
        private string name;

        [Tooltip("The audio clip to be played.")]
        [SerializeField]
        private AudioClip clip;

        [Tooltip("The volume of the audio. Ranges from 0 (silent) to 1 (maximum).")]
        [SerializeField, Range(0f, 1f)]
        private float volume = 1f;

        [Tooltip("The pitch of the audio. Ranges from 0.1 to 3.")]
        [SerializeField, Range(0.1f, 3f)]
        private float pitch = 1f;

        [Tooltip("Whether the audio should loop.")]
        [SerializeField]
        private bool loop = false;

        /// <summary>
        /// Gets or sets the name of the audio.
        /// </summary>
        public string Name
        {
            get => name;
            set => name = value;
        }

        /// <summary>
        /// Gets or sets the audio clip.
        /// </summary>
        public AudioClip Clip
        {
            get => clip;
            set => clip = value;
        }

        /// <summary>
        /// Gets or sets the volume of the audio.
        /// </summary>
        public float Volume
        {
            get => volume;
            set => volume = Mathf.Clamp(value, 0f, 1f); // Ensure volume is within valid range
        }

        /// <summary>
        /// Gets or sets the pitch of the audio.
        /// </summary>
        public float Pitch
        {
            get => pitch;
            set => pitch = Mathf.Clamp(value, 0.1f, 3f); // Ensure pitch is within valid range
        }

        /// <summary>
        /// Gets or sets whether the audio should loop.
        /// </summary>
        public bool Loop
        {
            get => loop;
            set => loop = value;
        }
    }
}
