using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    [SerializeField] private Player player;
    [Space(10)]
    [SerializeField] private CameraHandler playerCamera;
    [SerializeField] private PlayerSword playerSword;
    [SerializeField] private GameObject swordTrail;
    [SerializeField] private GameObject swordSlash;
    [SerializeField] private AudioSource playerWalkSound;

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
    private readonly float skillMpAmount = 80f;

    private bool canAttackCombo = false;
    private bool canAttackInput = true;

    public bool isInvis { get; set; } = false;
    public bool isWalk
    {
        get
        {
            if (isIgnoreInput || !canAttackInput)
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
        Player_ItemSwap();
        Player_UseItem();
        Player_Skill();
        AnimationUpdate();
        PlayerWalkSound();
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
                playerWalkSound.pitch = 1;
                player.playerStats.moveSpeed = player_DefaultSpeed;
                return;
            }
            MinusToStamina(runStaminaAmount);

            playerWalkSound.pitch = 2;
            player.playerStats.moveSpeed = player_SprintSpeed;
            SetPlayerState(PlayerState.Run);
        }
        else if (player.playerStats.moveSpeed != player_DefaultSpeed)
        {
            playerWalkSound.pitch = 1;
            player.playerStats.moveSpeed = player_DefaultSpeed;
            SetPlayerState(PlayerState.Idle);
        }
    }

    private void PlayerRoll()
    {
        if ((playerInput.isRoll && !isDisableAction) && isWalk)
        {
            if (!CompareToStamina(rollStaminaAmount)) return;

            isInvis = true;
            MinusToStamina(rollStaminaAmount);
            SoundManager.Inst.PlaySFX(SoundEffect.PlayerRoll, 0.7f);
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
            if (curAttackCount == 0)
            {
                curAttackCount++;
                yield return new WaitUntil(() => !isDisableAction);
            }
            else
            {
                if (curAttackCount >= 4) curAttackCount = 0;
                curAttackCount++;
            }

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
        SoundManager.Inst.PlaySFX(SoundEffect.PlayerSword);
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

    private void Player_UseItem()
    {
        if (playerInput.isItem && !isDisableAction)
        {
            var potion = player.potions[player.CurItemIndex];
            if (potion.count <= 0) return;

            potion.Use();
            potion.count--;
            PlayerUseItem_Animation();
        }
    }

    private void Player_Skill()
    {
        if (!CompareToMP(skillMpAmount)) return;
        if (!isExistSkill) return;

        if (playerInput.isSkill && !isDisableAction)
        {
            MinusToMP(skillMpAmount);
            PlayerSkill_Animation();
        }
    }

    private bool CompareToStamina(float amount)
    {
        if (player.Stamina > amount) return true;

        return false;
    }
    private bool CompareToMP(float amount)
    {
        if (player.MP > amount) return true;

        return false;
    }
    private void MinusToStamina(float amount)
    {
        player.Stamina -= amount;
    }
    private void MinusToMP(float amount)
    {
        player.MP -= amount;
    }

    private void PlayerWalkSound()
    {
        if (isWalk)
        {
            if (!playerWalkSound.isPlaying) playerWalkSound.Play();
        }
        else playerWalkSound.Stop();
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
