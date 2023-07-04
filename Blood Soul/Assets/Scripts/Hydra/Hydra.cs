using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
enum Anim
{
    Walk, EarthQuake, Bite, TaillAttack,
    Fire, Water, Posion, Die

}

public class Hydra : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    private Animator animator;
    private Rigidbody myRigidbody;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float[] attackSpeed;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody>();
    }

    [SerializeField] private float MoveRange;
    private void Update()
    {
        Move();
    }
    private void Move()
    {
        Vector3 pos = new Vector3(player.transform.position.x - transform.position.x , 0 , player.transform.position.z - transform.position.z);
        Vector3 velocity = pos.normalized * moveSpeed * Time.deltaTime;

        Vector3 targetPos = new Vector3(player.transform.position.x , transform.position.y, player.transform.position.z);
        transform.LookAt(targetPos);

        Collider[] isPlayer = Physics.OverlapBox(transform.position, Vector3.one * MoveRange, Quaternion.identity, LayerMask.GetMask("Player"));
        if(isPlayer.Length <= 0)
        {
            animator.SetBool((int)Anim.Walk, true);
            myRigidbody.velocity = velocity;
        }
    }
}
