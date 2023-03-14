using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private const string moveAxisX = "Horizontal";
    private const string moveAxisZ = "Vertical";

    public Vector3 moveInput { get; private set; }
    private bool isAttack => Input.GetMouseButtonDown(0);
    private bool isRoll => Input.GetKeyDown(KeyCode.LeftShift);
    private bool isSprint => Input.GetKeyDown(KeyCode.Space);
    private bool isJump => Input.GetKeyDown(KeyCode.C);

    private void Update()
    {
        moveInput = new Vector3(Input.GetAxisRaw(moveAxisX), 0, Input.GetAxisRaw(moveAxisZ)); 
    }

}