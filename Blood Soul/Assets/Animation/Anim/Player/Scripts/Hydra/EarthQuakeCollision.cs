using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EarthQuakeCollision : MonoBehaviour
{
    private ParticleSystem triggerEventType;

    private void Start()
    {
        triggerEventType = GetComponent<ParticleSystem>();
        triggerEventType.trigger.AddCollider(Hydra.instance.player.GetComponent<Collider>());
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Hit");
        }
    }
    private void OnParticleTrigger()
    {
        Debug.Log("Hit");
    }
}
