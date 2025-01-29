using SerapKeremGameTools._Game._ParticleEffectSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectTest : MonoBehaviour
{
    [SerializeField]
    private string[] particleEffectName;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ParticleEffectManager.Instance.PlayParticle(particleEffectName[0], transform.position, transform.rotation);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            ParticleEffectManager.Instance.PlayParticle(particleEffectName[1], transform.position, transform.rotation);
        }

    }
}
