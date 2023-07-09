using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootHold : MonoBehaviour
{
    [SerializeField] private float maxY;
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody rb;

    private bool isActive = false;

    private void FixedUpdate()
    {
        if(transform.position.y >= maxY)
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            isActive = false;
        }

        if (isActive)
        {
            rb.velocity = Vector3.up * speed;
        }       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isActive = true;
        }
    }
}
