using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    private void AnimationUpdate()
    {
        playerAnimator.SetBool("isRun", playerInput.isSprint);
        

    }

    private void AnimationUpdate(string name)
    {
        playerAnimator.SetTrigger(name);
    }
}
