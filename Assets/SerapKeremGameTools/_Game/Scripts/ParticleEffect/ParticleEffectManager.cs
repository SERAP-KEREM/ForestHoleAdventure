using SerapKeremGameTools._Game._Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SerapKeremGameTools._Game._ParticleEffectSystem
{
    public class ParticleEffectManager : MonoSingleton<ParticleEffectManager>
    {
        [Header("Particle Effect Configurations")]
        /// <summary>
        /// List of all particle effects data.
        /// </summary>
        [Tooltip("List of particle effect data configurations.")]
        [SerializeField]
        private List<ParticleEffectData> particleEffectDataList = new List<ParticleEffectData>();

        [Header("Particle Effect Pools")]
        /// <summary>
        /// Dictionary to store particle system pools for each particle effect.
        /// </summary>
        private Dictionary<string, Queue<ParticleSystem>> particlePools = new Dictionary<string, Queue<ParticleSystem>>();

        [Header("Particle Effect Parents")]
        /// <summary>
        /// Dictionary to store parent GameObjects for each particle effect group.
        /// </summary>
        private Dictionary<string, GameObject> particleEffectParents = new Dictionary<string, GameObject>();

        protected override void Awake()
        {
            base.Awake();
            InitializeParticlePools();
        }

        /// <summary>
        /// Initializes particle system pools for each particle effect defined in the data list.
        /// </summary>
        private void InitializeParticlePools()
        {
            foreach (var data in particleEffectDataList)
            {
                Queue<ParticleSystem> pool = new Queue<ParticleSystem>();

                // Create a parent GameObject for grouping particles
                GameObject effectParent = new GameObject(data.ParticleName);
                effectParent.transform.SetParent(transform);

                particleEffectParents[data.ParticleName] = effectParent;

                // Populate the pool
                for (int i = 0; i < data.ParticleCount; i++)
                {
                    var particlePrefab = data.ParticleSystem;

                    if (particlePrefab != null)
                    {
                        var instance = Instantiate(particlePrefab, effectParent.transform);
                        instance.gameObject.SetActive(false);
                        pool.Enqueue(instance);
                    }
                    else
                    {
#if UNITY_EDITOR
                        Debug.LogError($"Particle prefab not found for: {data.ParticleName}");
#endif
                    }
                }

                particlePools[data.ParticleName] = pool;
            }
        }

        /// <summary>
        /// Plays a particle effect at the specified position and rotation.
        /// </summary>
        /// <param name="particleName">Name of the particle effect to play.</param>
        /// <param name="position">Position where the effect will appear.</param>
        /// <param name="rotation">Rotation of the particle effect.</param>
        public void PlayParticle(string particleName, Vector3 position, Quaternion rotation)
        {
            if (particlePools.TryGetValue(particleName, out var pool) && pool.Count > 0)
            {
                var particle = pool.Dequeue();
                particle.transform.position = position;
                particle.transform.rotation = rotation;

                particle.gameObject.SetActive(true);
                particle.Play();

                StartCoroutine(ReturnToPoolAfterDuration(particle, particleName));
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning($"No available particle in the pool for: {particleName}");
#endif
            }
        }

        /// <summary>
        /// Stops all currently active particle effects.
        /// </summary>
        public void StopAllEffects()
        {
            ParticleSystem[] allParticles = GetComponentsInChildren<ParticleSystem>();
            foreach (var particle in allParticles)
            {
                particle.Stop();
            }
        }

        /// <summary>
        /// Returns a particle effect to its pool after completion.
        /// </summary>
        /// <param name="particleSystem">The particle system to return.</param>
        /// <param name="effectName">Name of the particle effect.</param>
        public void ReturnEffectToPool(ParticleSystem particleSystem, string effectName)
        {
            StartCoroutine(ReturnToPoolAfterDuration(particleSystem, effectName));
        }

        /// <summary>
        /// Coroutine to return a particle effect to the pool after its duration ends.
        /// </summary>
        /// <param name="particle">The particle system to return.</param>
        /// <param name="particleName">Name of the particle effect.</param>
        private IEnumerator ReturnToPoolAfterDuration(ParticleSystem particle, string particleName)
        {
            yield return new WaitForSeconds(particle.main.duration);

            particle.Stop();
            particle.gameObject.SetActive(false);

            if (particlePools.ContainsKey(particleName))
            {
                particlePools[particleName].Enqueue(particle);
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError($"No pool found for particle: {particleName}");
#endif
            }
        }
    }
}
