using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    [SerializeField] private Player player;
    [Space(10)]
    [SerializeField] private CameraHandler playerCamera;
    [SerializeField] private GameObject playerSword;

    private PlayerState curPlayerState = PlayerState.Idle;
    private PlayerInput playerInput;
    private Animator playerAnimator;
    private Rigidbody rigidBody;
    private Vector3 rotateDirection;

    private float turnSmoothTime;
    private float player_DefaultSpeed;
    private float player_SprintSpeed;
    private int curAttackCount = 0;

    private readonly float runStaminaAmount = 0.1f;
    private readonly float rollStaminaAmount = 15f;
    private readonly float attackStaminaAmount = 10f;

    private bool canAttackCombo = false;
    private bool canAttackInput = true;
    //private bool isSword = false;
    //private bool isInvis = false;
    public bool isWalk
    {
        get
        {
            if (isIgnoreInput || isDisableAction)
            {
                return false;
            }
            return playerInput.moveInput != Vector3.zero;
        }
    }
    public bool isExistSkill { get; set; } = false;
    public bool isIgnoreInput { get; set; } = false;
    public bool isDisableAction { get; set; } = false;

    private void Awake()
    {
        player = GetComponent<Player>();
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        PlayerControllerInit();
    }
    private void PlayerControllerInit()
    {
        turnSmoothTime = 5.5f;
        player_DefaultSpeed = player.playerStats.moveSpeed;
        player_SprintSpeed = player.playerStats.moveSpeed + 8f;
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
        PlayerAttack();
        AnimationUpdate();
        Player_ItemSwap();
    }

    private void PlayerMovement(Vector3 moveInput)
    {
        if (isIgnoreInput) return;

        var targetDirection = playerCamera.transform;
        var moveDirection = targetDirection.forward * moveInput.z + targetDirection.right * moveInput.x;
        moveDirection.y = 0;

        rotateDirection = moveDirection;
        moveDirection = moveDirection.normalized;

        var velocity = moveDirection * player.playerStats.moveSpeed + Vector3.up * rigidBody.velocity.y;
        rigidBody.velocity = velocity;

        if (isWalk && curPlayerState != PlayerState.Run) SetPlayerState(PlayerState.Walk);
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
        switch (curPlayerState)
        {
            case PlayerState.Idle:
                break;
            case PlayerState.Walk:
                PlayerWalk_Animation();
                break;
            case PlayerState.Run:
                break;
            case PlayerState.Roll:
                PlayerRoll_Animation();
                break;
            case PlayerState.Attack:
                PlayerAttack_Animation();
                break;
        }
    }

    public void SetPlayerState(PlayerState state)
    {
        if (!isDisableAction)
        {
            curPlayerState = state;
            PlayerStateMachine();
        }
        else if (canAttackCombo || state == PlayerState.Attack)
        {
            PlayerStateMachine();
        }
    }

    private void PlayerSprint()
    {
        if (playerInput.isSprint)
        {
            if (!CompareToStamina(runStaminaAmount))
            {
                player.playerStats.moveSpeed = player_DefaultSpeed;
                return;
            }
            MinusToStamina(runStaminaAmount);

            player.playerStats.moveSpeed = player_SprintSpeed;
            SetPlayerState(PlayerState.Run);
        }
        else if (player.playerStats.moveSpeed != player_DefaultSpeed)
        {
            player.playerStats.moveSpeed = player_DefaultSpeed;
            SetPlayerState(PlayerState.Idle);
        }
    }

    private void PlayerRoll()
    {
        if ((playerInput.isRoll && !isDisableAction) && isWalk)
        {
            //var rotation = Quaternion.LookRotation(rotateDirection.normalized);
            //rotation.y = 0;

            //transform.rotation = rotation;

            if (!CompareToStamina(rollStaminaAmount)) return;
            MinusToStamina(rollStaminaAmount);
            SetPlayerState(PlayerState.Roll);
        }
    }

    #region Attack

    private void PlayerAttack()
    {
        if (playerInput.isAttack)
        {
            if (!CompareToStamina(attackStaminaAmount)) return;

            if (curAttackCount == 0 || (canAttackCombo && canAttackInput))
            {
                StartCoroutine(AttackCoroutine());
            }
        }

        IEnumerator AttackCoroutine()
        {
            if (!canAttackCombo) yield return new WaitUntil(() => !isDisableAction);
            if (curAttackCount >= 4) curAttackCount = 0;

            curAttackCount++;
            canAttackInput = false;

            MinusToStamina(attackStaminaAmount);
            SetPlayerState(PlayerState.Attack);
            yield break;
        }
    }

    public void CanAttackCombo()
    {
        canAttackCombo = true;
        canAttackInput = true;
    }
    public void CanNotAttackCombo()
    {
        curAttackCount = 0;
        canAttackCombo = false;
        SetAnimationValue(false, false, false);
    }

    #endregion

    private void Player_ItemSwap()
    {
        if (playerInput.isItemSwap)
        {
            player.CurItemIndex = (player.CurItemIndex == 0) ? 1 : 0;
            UIManager.Inst.ItemUISwap(player.CurItemIndex);
        }
    }

    private bool CompareToStamina(float amount)
    {
        if (player.Stamina > amount) return true;

        return false;
    }
    private void MinusToStamina(float amount)
    {
        player.Stamina -= amount;
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
