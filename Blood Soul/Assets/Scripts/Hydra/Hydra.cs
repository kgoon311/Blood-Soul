using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

enum Pattern
{
    Breath,
    Earthquake,
    Bite,
    TailAttack,
}
public class Hydra : MonoBehaviour
{
    static public Hydra instance;
    public PlayerController player;

    private Animator myAnimator;
    private Rigidbody myRigidbody;

    [SerializeField] private bool isTestAttack;
    [SerializeField] private Pattern testType;

    private bool isMove = true;
    [SerializeField] private int phase = 0;
    [Header("Status")]
    [SerializeField] private float maxHp;
    [SerializeField] private float hp;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float[] attackSpeed;
    private float timer;

    [Header("Attack")]
    private int beforeParttern = -1;

    [Header("Bite")]
    [SerializeField] private float bite_AngleRange = 30f;
    [SerializeField] private float bite_Radius = 3f;

    [Header("Tail")]
    [SerializeField] private Collider[] tailColider;
    private bool isTailHit;

    [Header("Earth")]
    [SerializeField] private ParticleSystem earthQuakePS;
    [SerializeField] private GameObject[] footPos;
    private bool isEarthQuakeHit;
    private bool isLeftFoot = false;

    [Header("Breath")]
    [SerializeField] private float breathPatternRange;
    [SerializeField] private GameObject[] breathPS;
    [SerializeField] private GameObject[] chargerPS;
    [SerializeField] private GameObject[] headPos;
    private void Awake()
    {
        instance = this;

        myAnimator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody>();

        hp = maxHp;
        timer = Random.Range(attackSpeed[0], attackSpeed[1]);
    }

    [SerializeField] private float MoveRange;
    private void Update()
    {
        if (isMove == true)
        {
            Move();
            AttackTimer();
        }
        if (isTestAttack &&Input.GetKeyDown(KeyCode.Q))
        {
            PlayAttackAnim(testType);
        }

        float percent = hp / maxHp;
        
        if (percent < 0.6f)
        {
            phase = 1;
        }
        else if(percent < 0.3f)
            phase = 2;

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
            myAnimator.SetBool("Walk", true);
            myRigidbody.velocity = new Vector3(velocity.x, myRigidbody.velocity.y, velocity.z);
        }
        else
        {
            myAnimator.SetBool("Walk", false);
            myRigidbody.velocity = Vector3.zero;
        }
    }
    private void AttackTimer()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            float dis = Vector3.Distance(transform.position, player.transform.position);

            if (dis > breathPatternRange)
            {
                if (phase == 0)
                    beforeParttern = 0;
                else
                    beforeParttern = Random.Range(0, 2);

            }
            else
            { 
                    beforeParttern = Random.Range(2, 4);
            }

            PlayAttackAnim((Pattern)beforeParttern);
            myRigidbody.velocity = Vector3.zero;
            timer = Random.Range(attackSpeed[0], attackSpeed[1]);
        }
    }

    private void PlayAttackAnim(Pattern pattern)
    {
        switch (pattern)
        {
            case Pattern.Breath:
                {
                    if(phase == 0)
                        myAnimator.SetTrigger("Single");
                    else if(phase == 1)
                        myAnimator.SetTrigger("Double");
                    else if(phase == 2)
                        myAnimator.SetTrigger("Triple");

                    StartCoroutine(BreathAttack());
                    break;
                }
            case Pattern.Bite:
                {
                    myAnimator.SetTrigger("Bite");
                    StartCoroutine(BiteAttack());
                    break;
                }
            case Pattern.TailAttack:
                {
                    myAnimator.SetTrigger("TaillAttack");
                    StartCoroutine(TailAttack());
                    break;
                }
            case Pattern.Earthquake:
                {
                    myAnimator.SetTrigger("EarthQuake");
                    StartCoroutine(EarthQuake());
                    break;
                }
        }
    }

    ///////////////// - Attack Pattern - //////////////////////
    private IEnumerator BiteAttack()
    {
        isMove = false;

        yield return new WaitForSeconds(0.7f);

        bool hit = BiteCircularSector(bite_Radius, bite_AngleRange);
        if (hit)
            Debug.Log("Bite Hit");

        yield return new WaitForSeconds(1f);

        isMove = true;
    }
    private bool BiteCircularSector(float radius, float range)
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

    private IEnumerator TailAttack()
    {
        isMove = false;

        for (int i = 0; i < tailColider.Length; i++)
            tailColider[i].enabled = true;

        yield return new WaitForSeconds(2.5f);
        for (int i = 0; i < tailColider.Length; i++)
            tailColider[i].enabled = false;

        isMove = true;
        isTailHit = false;
    }

    private void TailCollision()
    {
        if (isTailHit == false)
        {
            isTailHit = true;
            Debug.Log("Tail Hit");
        }
    }

    private IEnumerator EarthQuake()
    {
        isMove = false;

        yield return new WaitForSeconds(8.2f);

        isMove = true;
        isEarthQuakeHit = false;
    }
    public void EarthQuakeCollision()
    {
        if (isEarthQuakeHit == false)
        {
            isEarthQuakeHit = true;
            Debug.Log("EarthQuake Hit");
        }
    }
    private IEnumerator BreathAttack()
    {
        isMove = false;

        yield return new WaitForSeconds(1.5f);

        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime / 3;
            Vector3 dir = headPos[0].transform.position - headPos[1].transform.position;
            if (Physics.Raycast(headPos[0].transform.position, dir * 20, 100, LayerMask.GetMask("Player")) == true)
            {
                Debug.Log("hit");
            }
            yield return null;
        }

        isMove = true;
        yield return null;
    }

    ///////////////// - Anim Trigger - //////////////////////
    public void EarthQuakeParticle()
    {
        Instantiate(earthQuakePS, isLeftFoot ? footPos[0].transform.position : footPos[1].transform.position, Quaternion.Euler(90, 0, 0));
        isLeftFoot = !isLeftFoot;
    }
    public void LaserParticle()
    {
        StartCoroutine(BreathParticlePlay(phase));
    }
    private IEnumerator BreathParticlePlay(int idx)
    {
        switch (idx)
        {
            case 0:
                {
                    chargerPS[1].SetActive(true); 
                    yield return new WaitForSeconds(1.5f);
                    breathPS[1].SetActive(true);
                    break;
                }
            case 1:
                {
                    chargerPS[0].SetActive(true);
                    chargerPS[2].SetActive(true);
                    yield return new WaitForSeconds(1.5f);
                    breathPS[0].SetActive(true);
                    breathPS[2].SetActive(true);
                    break;
                }
            case 2:
                {
                    chargerPS[0].SetActive(true);
                    chargerPS[1].SetActive(true);
                    chargerPS[2].SetActive(true);
                    yield return new WaitForSeconds(1.5f);
                    breathPS[0].SetActive(true);
                    breathPS[1].SetActive(true);
                    breathPS[2].SetActive(true);
                    break;
                }
        }
        yield return new WaitForSeconds(3.5f);
        switch (idx)
        {
            case 0:
                {
                    chargerPS[1].SetActive(false);
                    breathPS[1].SetActive(false);
                    break;
                }
            case 1:
                {
                    chargerPS[0].SetActive(false);
                    chargerPS[2].SetActive(false);
                    breathPS[0].SetActive(false);
                    breathPS[2].SetActive(false);
                    break;
                }
            case 2:
                {
                    chargerPS[0].SetActive(false);
                    chargerPS[1].SetActive(false);
                    chargerPS[2].SetActive(false);
                    breathPS[0].SetActive(false);
                    breathPS[1].SetActive(false);
                    breathPS[2].SetActive(false);
                    break;
                }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            TailCollision();
    }
}
