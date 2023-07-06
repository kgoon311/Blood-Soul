using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    private void AnimationUpdate()
    {
        if (isIgnoreInput || isDisableAction) playerAnimator.SetBool("isWalk", false);
        else playerAnimator.SetBool("isWalk", isMove);

        if (!isMove) playerAnimator.SetBool("isRun", false);
        else playerAnimator.SetBool("isRun", playerInput.isSprint);
    }

    private void SetAnimation(string name, float fadeOut, bool disable = false, bool rootMotion = false, bool ignore = false)
    {
        isDisableAction = disable;
        isIgnoreInput = ignore;
        playerAnimator.applyRootMotion = rootMotion;

        playerAnimator.CrossFade(name, fadeOut);
    }

    private void SetAnimation(string name, bool disable = false, bool rootMotion = false, bool ignore = false)
    {
        isDisableAction = disable;
        isIgnoreInput = ignore;
        playerAnimator.applyRootMotion = rootMotion;

        playerAnimator.Play(name);
    }

    private void PlayerRoll_Animation() => SetAnimation("Player_Roll", true, true, true);

}
