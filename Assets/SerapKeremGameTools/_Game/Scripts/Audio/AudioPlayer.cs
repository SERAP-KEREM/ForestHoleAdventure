using UnityEngine;
using System.Collections;

namespace SerapKeremGameTools._Game._AudioSystem
{
    /// <summary>
    /// Manages the audio playback for each AudioPlayer instance.
    /// </summary>
    public class AudioPlayer : MonoBehaviour
    {
        private AudioSource audioSource;

        void Awake()
        {
            // Initialize the AudioSource component
            audioSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Plays the given audio clip with the specified settings.
        /// </summary>
        /// <param name="audio">Audio settings to play</param>
        public void PlayAudio(Audio audio, bool loop)
        {
            if (!audioSource.isPlaying)  // If the audio is not already playing
            {
                audioSource.clip = audio.Clip;
                audioSource.volume = audio.Volume;  // Set volume
                audioSource.pitch = audio.Pitch;    // Set pitch
                audioSource.loop = loop;            // Set the loop based on the parameter
                audioSource.Play();

                // If the audio doesn't loop, return it to the pool after it finishes
                if (!loop)
                {
                    StartCoroutine(ReturnToPoolAfterPlaying(audio));
                }
            }
        }



        /// <summary>
        /// Waits until the audio clip finishes and returns the AudioPlayer to the pool.
        /// </summary>
        private IEnumerator ReturnToPoolAfterPlaying(Audio audio)
        {
            yield return new WaitForSeconds(audio.Clip.length);
            AudioManager.Instance.ReturnAudioPlayerToPool(this);
        }
    }
}
