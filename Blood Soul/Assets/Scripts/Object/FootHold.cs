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
        if (isActive && transform.position.y < maxY)
        {
            rb.velocity = Vector3.up * speed;
        }
        else if(maxY <= transform.position.y)
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            isActive = false;
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
