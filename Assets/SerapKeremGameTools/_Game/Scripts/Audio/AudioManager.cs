using UnityEngine;
using System.Collections.Generic;
using SerapKeremGameTools._Game._objectPool;
using SerapKeremGameTools._Game._Singleton;
using SerapKeremGameTools._Game._SaveLoadSystem;

namespace SerapKeremGameTools._Game._AudioSystem
{
    /// <summary>
    /// Manages audio playback, pooling for AudioPlayers, and provides playback controls.
    /// This class ensures that audio is efficiently managed and reused in the game.
    /// </summary>
    public class AudioManager : MonoSingleton<AudioManager>
    {
        [Header("Audio Clips List")]
        [Tooltip("A list of all available audio clips that can be played.")]
        [SerializeField]
        private List<Audio> audioClips = new List<Audio>(); // List to store audio clips

        [Header("AudioPlayer Prefab")]
        [Tooltip("The AudioPlayer prefab used to play audio.")]
        [SerializeField]
        private AudioPlayer audioPlayerPrefab; // Reference to the AudioPlayer prefab

        private ObjectPool<AudioPlayer> audioPlayerPool; // Object pool for AudioPlayers
        [SerializeField]
        [Tooltip("The maximum number of AudioPlayers that can be in the pool.")]
        private int poolSize = 10; // Maximum number of AudioPlayers in the pool

        // Holds the currently playing audio name
        private string currentAudio = string.Empty;

        /// <summary>
        /// Initializes the AudioManager instance and sets up the audio pool.
        /// Ensures only one instance of AudioManager exists and loads audio clips.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            // Create the audio player pool with a capacity of poolSize
            InitializeAudioPlayerPool();
            ApplySavedVolume();
            // Load the audio clips from Resources folder
            LoadAudioClips();
        }

        /// <summary>
        /// Initializes the ObjectPool for AudioPlayers with a specified pool size.
        /// </summary>
        private void InitializeAudioPlayerPool()
        {
            audioPlayerPool = new ObjectPool<AudioPlayer>(audioPlayerPrefab, poolSize, transform);
        }
        private void ApplySavedVolume()
        {
            float savedVolume = LoadManager.LoadData<float>("MusicVolume", 1f);
            AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
            foreach (var source in audioSources)
            {
                source.volume = savedVolume;
            }
        }

        /// <summary>
        /// Loads all audio clips from the Resources/Audio folder.
        /// </summary>
        private void LoadAudioClips()
        {
            // Load all AudioClips from Resources/Audio
            AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio");
            foreach (var clip in clips)
            {
                Audio newAudio = new Audio()
                {
                    Name = clip.name,
                    Clip = clip,
                    Volume = 1f,
                    Pitch = 1f,
                    Loop = true
                };
                audioClips.Add(newAudio);
            }
        }

        /// <summary>
        /// Plays an audio clip by its name from the audioClips list.
        /// If the audio is already playing, it won't play again.
        /// </summary>
        /// <param name="audioName">The name of the audio clip to play.</param>
        public void PlayAudio(string audioName, bool loop = false)
        {
            // Find the audio clip by name
            Audio audio = audioClips.Find(a => a.Name == audioName);
            if (audio != null)
            {
                // Check if the audio is already playing
                if (currentAudio == audioName && loop)
                {
#if UNITY_EDITOR
                    Debug.Log($"Audio {audioName} is already playing in loop.");
#endif
                    return;
                }

                // Get an AudioPlayer from the pool and play the audio
                AudioPlayer audioPlayer = audioPlayerPool.GetObject();
                audioPlayer.PlayAudio(audio, loop);  // Pass the loop parameter to the PlayAudio method

                // Set the current playing audio to this one
                currentAudio = audioName;
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning($"Audio not found: {audioName}");
#endif
            }
        }



        /// <summary>
        /// Pauses all active AudioSources in the scene.
        /// This is useful for pausing all audio during a pause menu or when switching scenes.
        /// </summary>
        public void PauseAllAudio()
        {
            AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
            foreach (var source in audioSources)
            {
                if (source.isPlaying)
                {
                    source.Pause();
                }
            }
        }

        /// <summary>
        /// Resumes all paused AudioSources in the scene.
        /// This resumes the playback of any paused audio.
        /// </summary>
        public void ResumeAllAudio()
        {
            AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
            foreach (var source in audioSources)
            {
                if (!source.isPlaying)
                {
                    source.UnPause();
                }
            }
        }

        /// <summary>
        /// Checks if the audio clip with the given name is currently playing.
        /// </summary>
        /// <param name="audioName">The name of the audio clip to check.</param>
        /// <returns>True if the audio is playing, otherwise false.</returns>
        public bool IsPlaying(string audioName)
        {
            // Return true if the given audio is the one currently playing
            return currentAudio == audioName;
        }

        /// <summary>
        /// Returns the AudioPlayer to the pool after it has finished playing.
        /// </summary>
        /// <param name="audioPlayer">The AudioPlayer to return to the pool.</param>
        public void ReturnAudioPlayerToPool(AudioPlayer audioPlayer)
        {
            audioPlayerPool.ReturnObject(audioPlayer);
        }
    }
}
