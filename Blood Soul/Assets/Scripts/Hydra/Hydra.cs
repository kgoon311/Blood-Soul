using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

enum Pattern
{
    SingleBreath,
    DoubleBreath,
    TripleBreath,
    Bite,
    TailAttack,
    Earthquake,
    PosionZone

}
public class Hydra : MonoBehaviour
{
    static public Hydra instance;
    [SerializeField] public PlayerController player;
    private Animator animator;
    private Rigidbody myRigidbody;

    [SerializeField] private bool isTestAttack;
    [SerializeField] private Pattern testType;
    
    private bool isMove = true;

    [Header("Status")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float[] attackSpeed;

    [Header("Attack")]
    [SerializeField] private int attackPatternRange;
    [SerializeField] private List<GameObject> attackCollider = new List<GameObject>();
    private int beforeParttern;
    
    [Header("Bite")]
    [SerializeField] private float bite_AngleRange = 30f;
    [SerializeField] private float bite_Radius = 3f;

    [Header("Tail")]
    [SerializeField] private Collider[] tailColider;
    private bool isTailHit;

    [Header("Earth")]
    [SerializeField] private ParticleSystem earthQuakeParticle;
    [SerializeField] private GameObject[] footPos;
    private bool isLeftFoot = false;
    private void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody>();
    }

    [SerializeField] private float MoveRange;
    private void Update()
    {
        if(isMove == true)
            Move();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AttackAnim(testType);
            //StartCoroutine(TailAttack());
        }
    }
    private void Move()
    {
        Vector3 pos = new Vector3(player.transform.position.x - transform.position.x, 0, player.transform.position.z - transform.position.z);
        Vector3 velocity = pos.normalized * moveSpeed * Time.deltaTime;

        Vector3 targetPos = new Vector3(player.transform.position.x - transform.position.x, transform.position.y, player.transform.position.z - transform.position.z);
        Quaternion targetRotation = Quaternion.LookRotation(targetPos);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        Collider[] isPlayer = Physics.OverlapBox(transform.position, Vector3.one * MoveRange, Quaternion.identity, LayerMask.GetMask("Player"));
        if (isPlayer.Length <= 0)
        {
            animator.SetBool("Walk", true);
            myRigidbody.velocity = new Vector3(velocity.x, myRigidbody.velocity.y, velocity.z);
        }
        else
        {
            animator.SetBool("Walk", false);
            myRigidbody.velocity = Vector3.zero;
        }
    }
    private void AttackAnim(Pattern pattern)
    {
        switch (pattern)
        {
            case Pattern.SingleBreath:
                {
                    animator.SetTrigger("Single");
                    break;
                }
            case Pattern.DoubleBreath:
                {
                    animator.SetTrigger("Double");
                    break;
                }
            case Pattern.TripleBreath:
                {
                    animator.SetTrigger("Triple");
                    break;  
                }
            case Pattern.Bite:
                {
                    animator.SetTrigger("Bite");
                    break;
                }
            case Pattern.TailAttack:
                {
                    animator.SetTrigger("TaillAttack");
                    break;
                }
            case Pattern.Earthquake:
                {
                    animator.SetTrigger("EarthQuake");
                    break;
                }
            case Pattern.PosionZone:
                {
                    animator.SetTrigger("Posion");
                    break;
                }
        }
    }

    private IEnumerator BiteAttack()
    {
        isMove = false;

        yield return new WaitForSeconds(0.1f);

        AttackAnim(Pattern.Bite);
        
        yield return new WaitForSeconds(0.6f);

        bool hit = CircularSector(bite_Radius,bite_AngleRange);
        if (hit)
            Debug.Log("Bite Hit");

        yield return new WaitForSeconds(1f);
    
        isMove = true;
    }

    private IEnumerator TailAttack()
    {
        isMove = false;

        yield return new WaitForSeconds(0.1f);

        AttackAnim(Pattern.TailAttack);
        for(int i = 0; i< tailColider.Length;i++)
            tailColider[i].enabled = true;

        yield return new WaitForSeconds(2.5f);
        for (int i = 0; i < tailColider.Length; i++)
            tailColider[i].enabled = false;
        
        isMove = true;
        isTailHit = false;
    }
    private void TailCollision()
    {
        if(isTailHit == false)
        {
            isTailHit = true;
            Debug.Log("Tail Hit");
        }    
    }
    private IEnumerator Earthquake()
    {

        yield return new WaitForSeconds(1f);
    }
    public void EarthQuakeParticle()
    {
        Instantiate(earthQuakeParticle, isLeftFoot ? footPos[0].transform.position : footPos[1].transform.position , Quaternion.Euler(90,0,0));
        isLeftFoot = !isLeftFoot;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            TailCollision();
    }

    private bool CircularSector(float radius,float range)
    {
        Vector3 interV = player.transform.position - transform.position;

        if (interV.magnitude <= radius)
        {
            //플레이어와 히드라의 각도를 구한다
            float dot = Vector3.Dot(interV.normalized, transform.forward);
            float theta = Mathf.Acos(dot);
            float degree = Mathf.Rad2Deg * theta;

            if (degree <= range / 2f)
                return true;
            else
                return false;

        }
        else
            return false;
    }
}
