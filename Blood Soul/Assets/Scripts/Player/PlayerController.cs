using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    public float health = 0;
    public float stamina = 0;
    public float moveSpeed = 0;
    public float jumpForce = 0;
    public float attackDmg = 0;

    public PlayerStats()
    {
        health = 200;
        stamina = 100;
        moveSpeed = 10;
        jumpForce = 8;
        attackDmg = 20;
    }
}

public partial class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [Space(10)]
    [SerializeField] private CameraHandler playerCamera;
    [SerializeField] private GameObject playerSword;
    [SerializeField] private Transform player_HeadTrasnfrom;
    [SerializeField] private Transform player_HandTransform;
    [SerializeField] private Transform player_SwordHolderTransform;

    private PlayerState curPlayerState = PlayerState.Idle;
    private PlayerInput playerInput;
    private Animator playerAnimator;
    private Rigidbody rigidBody;
    private Vector3 rotateDirection;
    private Vector3 rollDirection;

    private float turnSmoothTime;
    private float player_DefaultSpeed;
    private float player_SprintSpeed;
    private int attackCount;

    private bool isSword = false;
    private bool isInvis = false;
    public bool isMove
    {
        get
        {
            return playerInput.moveInput != Vector3.zero;
        }
    }
    public bool isIgnoreInput { get; set; } = false;
    public bool isDisableAction { get; set; } = false;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        PlayerInit();
    }
    private void PlayerInit()
    {
        playerStats = new PlayerStats();

        turnSmoothTime = 5.5f;
        player_DefaultSpeed = playerStats.moveSpeed;
        player_SprintSpeed = playerStats.moveSpeed + 9f;
    }

    private void FixedUpdate()
    {
        PlayerMovement(playerInput.moveInput);
        PlayerRotate(rotateDirection);
    }
    private void Update()
    {
        PlayerSprint();
        PlayerRoll();
        AnimationUpdate();
    }

    private void PlayerMovement(Vector3 moveInput)
    {
        if (isIgnoreInput) return;

        var targetDirection = playerCamera.transform;
        var moveDirection = targetDirection.forward * moveInput.z + targetDirection.right * moveInput.x;
        moveDirection.y = 0;

        rotateDirection = moveDirection;
        rollDirection = moveDirection.normalized;
        moveDirection = moveDirection.normalized;

        var velocity = moveDirection * playerStats.moveSpeed + Vector3.up * rigidBody.velocity.y;
        rigidBody.velocity = velocity;

    }

    private void PlayerRotate(Vector3 direction)
    {
        Vector3 targetDir = direction;
        targetDir.y = 0;

        if (targetDir == Vector3.zero) targetDir = transform.forward;

        Quaternion rotationValue = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, rotationValue, Time.deltaTime * turnSmoothTime);

        transform.rotation = targetRotation;
    }

    private void PlayerStateMachine()
    {
        if (isDisableAction) return;

        switch (curPlayerState)
        {
            case PlayerState.Idle:
                break;
            case PlayerState.Walk:
                break;
            case PlayerState.Run:
                break;
            case PlayerState.Roll:
                PlayerRoll_Animation();
                break;
            case PlayerState.Attack: break;
        }
    }

    public void SetPlayerState(PlayerState state)
    {
        if (curPlayerState != state)
        {
            curPlayerState = state;

            PlayerStateMachine();
        }
    }

    private void PlayerSprint()
    {
        if (playerInput.isSprint)
        {
            playerStats.moveSpeed = player_SprintSpeed;
            SetPlayerState(PlayerState.Run);
        }
        else if (playerStats.moveSpeed != player_DefaultSpeed)
        {
            playerStats.moveSpeed = player_DefaultSpeed;
            SetPlayerState(PlayerState.Idle);
        }
    }

    private void PlayerRoll()
    {
        if ((playerInput.isRoll && !isDisableAction) && isMove)
        {
            //var rotation = Quaternion.LookRotation(rollDirection);
            //rotation.y = 0;

            //transform.rotation = rotation;
            SetPlayerState(PlayerState.Roll);
        }
    }

    private void PlayerAttack()
    {

    }
}

public enum PlayerState
{
    Idle,
    Walk,
    Run,
    Roll,
    Attack
}
