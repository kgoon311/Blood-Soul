using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    [SerializeField] private Collider swordCollider;
    [SerializeField] private float attackDamage;

    public void ColliderOn() => swordCollider.enabled = true;
    public void ColliderOff() => swordCollider.enabled = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Hydra>().GetDamage(attackDamage);
        }
    }
}
