using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    private void AnimationUpdate()
    {
        playerAnimator.SetBool("isRun", playerInput.isSprint);
        playerAnimator.SetBool("isWalk", (playerInput.moveInput == Vector3.zero) ? false : true);

        //if ((playerInput.isSprint || playerInput.isAttack) && !isSword)
        //{
        //    playerAnimator.SetTrigger("drawSword");
        //    isSword = true;
        //}
    }

    private void AnimationUpdate(string name)
    {
        playerAnimator.SetTrigger(name);
    }

    private void PlayerDrawSword()
    {
        playerSword.transform.position = player_HandTransform.position;
        playerSword.transform.rotation = Quaternion.Euler
            (new Vector3(player_HandTransform.rotation.x, playerSword.transform.rotation.y, player_HandTransform.rotation.z));
        playerSword.transform.SetParent(player_HandTransform);
    }
    private void PlayerSheathSword()
    {

    }

}
