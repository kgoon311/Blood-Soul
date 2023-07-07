using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class PlayerIdle_StateMachine : StateMachineBehaviour
{
    private PlayerController controller;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.GetComponent<PlayerController>();

        controller.SetPlayerState(PlayerState.Idle);
    }
}
