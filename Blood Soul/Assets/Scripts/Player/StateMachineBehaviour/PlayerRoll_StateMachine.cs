using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoll_StateMachine : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.applyRootMotion = false;
    }
}
