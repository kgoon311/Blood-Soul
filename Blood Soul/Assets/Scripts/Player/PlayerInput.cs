using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private readonly string MOVE_AXIS_X = "Horizontal";
    private readonly string MOVE_AXIS_Z = "Vertical";

    private readonly KeyCode JUMP_KEY = KeyCode.F;
    private readonly KeyCode SPRINT_KEY = KeyCode.LeftShift;
    private readonly KeyCode ROLL_KEY = KeyCode.Space;
    private readonly KeyCode ITEM_KEY = KeyCode.R;

    public Vector3 moveInput { get; private set; }
    public bool isJump = false;
    public bool isSprint = false;
    public bool isRoll = false;
    public bool isItem = false;
    public bool isAttack = false;

    private void Update()
    {
        moveInput = new Vector3(Input.GetAxisRaw(MOVE_AXIS_X), 0, Input.GetAxisRaw(MOVE_AXIS_Z));

        isJump = Input.GetKeyDown(JUMP_KEY);
        isSprint = Input.GetKey(SPRINT_KEY);
        isRoll = Input.GetKeyDown(ROLL_KEY);
        isItem = Input.GetKeyDown(ITEM_KEY);
        isAttack = Input.GetMouseButtonDown(0);
    }

}