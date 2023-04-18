using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private const string MOVE_AXIS_X = "Horizontal";
    private const string MOVE_AXIS_Z = "Vertical";

    public Vector3 moveInput { get; private set; }
    

    private void Update()
    {
        moveInput = new Vector3(Input.GetAxisRaw(MOVE_AXIS_X), 0, Input.GetAxisRaw(MOVE_AXIS_Z)); 
    }

}