using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRollBack_StateMachine : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var controller = animator.GetComponent<PlayerController>();

        controller.isDisableAction = false;
        controller.isIgnoreInput = false;
        controller.isInvis = false;
        animator.applyRootMotion = false;
    }
}
