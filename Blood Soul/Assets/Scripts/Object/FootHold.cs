using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootHold : MonoBehaviour
{
    [SerializeField] private float maxY;
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private AudioSource audioClip;

    private bool isActive = false;
    private bool isOne = false;

    private void FixedUpdate()
    {
        if (isActive)
        {
            if (transform.position.y < maxY)
            {
                rb.velocity = Vector3.up * speed;
            }
            else if (maxY <= transform.position.y)
            {
                isActive = false;
                rb.isKinematic = true;

                rb.velocity = Vector3.zero;
                Player.Inst.GetComponent<Rigidbody>().velocity = Vector3.zero;
                audioClip.Stop();
            }
        }  
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isOne)
        {
            isActive = true;
            isOne = true;
            audioClip.Play();
        }
    }
}
