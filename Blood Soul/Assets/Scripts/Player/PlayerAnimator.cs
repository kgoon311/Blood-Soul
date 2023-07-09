using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    private void AnimationUpdate()
    {
        if (!isWalk || playerAnimator.applyRootMotion) playerAnimator.SetBool("isWalk", false);
        else playerAnimator.SetBool("isWalk", isWalk);

        if (!isWalk || !CompareToStamina(runStaminaAmount)) playerAnimator.SetBool("isRun", false);
        else playerAnimator.SetBool("isRun", playerInput.isSprint);
    }

    public void SetAnimationValue(bool disable = false, bool rootMotion = false, bool ignore = false)
    {
        isDisableAction = disable;
        isIgnoreInput = ignore;
        playerAnimator.applyRootMotion = rootMotion;
    }
    private void PlayTargetAnimation(string name, bool disable = false, bool rootMotion = false, bool ignore = false)
    {
        isDisableAction = disable;
        isIgnoreInput = ignore;
        playerAnimator.applyRootMotion = rootMotion;

        playerAnimator.Play(name);
    }
    private void PlayTargetAnimation(string name, float fadeOut, bool disable = false, bool rootMotion = false, bool ignore = false)
    {
        isDisableAction = disable;
        isIgnoreInput = ignore;
        playerAnimator.applyRootMotion = rootMotion;

        playerAnimator.CrossFade(name, fadeOut);
    }

    private void PlayerRoll_Animation() => PlayTargetAnimation("Player_Roll", true, true, true);

    private void PlayerWalk_Animation() => PlayTargetAnimation("Player_Walk", false, false, false);

    private void PlayerAttack_Animation()
    {
        if (curAttackCount > 1)
            PlayTargetAnimation($"Player_Attack{curAttackCount}", 0.1f, true, true, true);
        else
            PlayTargetAnimation($"Player_Attack{curAttackCount}", true, true, true);
    }

    private void PlayerUseItem_Animation()
    {
        SetAnimationValue(true, false, false);
        playerAnimator.SetTrigger("isDrink");
    }

    public void PlayerItemEffect() => player.potions[player.CurItemIndex].effect.gameObject.SetActive(true);
    public void OnSwordTrail() => swordTrail.gameObject.SetActive(true);
    public void OffSwordTrail() => swordTrail.gameObject.SetActive(false);

    private void PlayerSkill_Animation()
    {

    }
    public void PlayerSkillEffect()
    {

    }
}
