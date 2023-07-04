using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    private void AnimationUpdate()
    {
        if(!isIgnoreInput && !isDisableAction)
            playerAnimator.SetBool("isWalk", isMove);

        playerAnimator.SetBool("isRun", playerInput.isSprint);
    }

    private void SetAnimation(string name, float fadeOut, bool disable = false, bool rootMotion = false, bool ignore = false)
    {
        isDisableAction = disable;
        isIgnoreInput = ignore;
        playerAnimator.applyRootMotion = rootMotion;

        playerAnimator.CrossFade(name, fadeOut);
    }

    private void PlayerRoll_Animation() => SetAnimation("Player_Roll", 0.3f, true, true, true);
}
