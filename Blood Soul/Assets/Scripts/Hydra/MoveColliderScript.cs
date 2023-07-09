using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveColliderScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Hydra.instance.MoveStart();
        }
    }
}
