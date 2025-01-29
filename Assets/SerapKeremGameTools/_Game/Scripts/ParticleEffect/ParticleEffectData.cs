using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SerapKeremGameTools._Game._ParticleEffectSystem
{
    [System.Serializable]
    public class ParticleEffectData
    {
        [Header("Particle Settings")]
        [Tooltip("The name of the particle effect.")]
        [SerializeField]
        private string particleName;

        [Tooltip("The number of particles to be generated.")]
        [SerializeField]
        private int particleCount;

        [Tooltip("The list of particle system prefabs.")]
        [SerializeField]
        private List<ParticleSystem> particleSystemList;

        /// <summary>
        /// Gets the name of the particle effect.
        /// </summary>
        public string ParticleName
        {
            get { return particleName; }
            private set { particleName = value ?? throw new System.ArgumentNullException(nameof(value)); }
        }

        /// <summary>
        /// Gets the number of particles to be generated.
        /// </summary>
        public int ParticleCount
        {
            get { return particleCount; }
            private set { particleCount = value > 0 ? value : throw new System.ArgumentOutOfRangeException(nameof(value), "Particle count must be greater than zero."); }
        }

        /// <summary>
        /// Gets a random particle system from the list of particle systems.
        /// This property selects a random particle effect from the list.
        /// </summary>
        public ParticleSystem ParticleSystem => particleSystemList[Random.Range(0, particleSystemList.Count)];
    }
}

