using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private readonly string MOVE_AXIS_X = "Horizontal";
    private readonly string MOVE_AXIS_Z = "Vertical";

    private readonly KeyCode SPRINT_KEY = KeyCode.LeftShift;
    private readonly KeyCode ROLL_KEY = KeyCode.Space;
    private readonly KeyCode ITEM_KEY = KeyCode.R;
    private readonly KeyCode ITEM_SWAP_KEY = KeyCode.DownArrow;

    public Vector3 moveInput { get; private set; }

    public bool isSprint { get; private set; } = false;
    public bool isRoll { get; private set; } = false;
    public bool isAttack { get; private set; } = false;
    public bool isItem { get; private set; } = false;
    public bool isItemSwap { get; private set; } = false;
    public bool isSkill { get; private set; } = false;

    private void Update()
    {
        moveInput = new Vector3(Input.GetAxisRaw(MOVE_AXIS_X), 0, Input.GetAxisRaw(MOVE_AXIS_Z));

        isSprint = Input.GetKey(SPRINT_KEY);
        isRoll = Input.GetKeyDown(ROLL_KEY);
        isItem = Input.GetKeyDown(ITEM_KEY);
        isItemSwap = Input.GetKeyDown(ITEM_SWAP_KEY);
        isSkill = (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButton(0));
        isAttack = (Input.GetMouseButtonDown(0) && !isSkill);
    }
}