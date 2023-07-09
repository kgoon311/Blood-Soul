using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    [SerializeField] private ParticleSystem playerSkill;
    [SerializeField] private float skillDamage;

    private void Awake()
    {
        playerSkill = GetComponent<ParticleSystem>();
        playerSkill.trigger.AddCollider(Hydra.instance.GetComponent<Collider>());
    }

    private void OnParticleTrigger()
    {
        Hydra.instance.GetDamage(skillDamage);
    }
}
