using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathCollision : MonoBehaviour
{
    private ParticleSystem triggerEventType;

    private void Start()
    {
        triggerEventType = GetComponent<ParticleSystem>();
        triggerEventType.trigger.AddCollider(Hydra.instance.player.GetComponent<Collider>());
    }
    private void OnParticleTrigger()
    {
        Hydra.instance.BreathCollision();
    }
}
