using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    private const float HORIZONTAL_MIN_ROTATION = -30f;
    private const float HORIZONTAL_MAX_ROTATION = 60f;

    [SerializeField] private Transform Camera;
    [SerializeField] private Transform target;

    [SerializeField] private float rotationSpeed;
    [SerializeField] private float lookSpeed;
    [SerializeField] private float moveSpeed;

    private Vector3 lookSmoothVelocity;
    private Vector3 moveSmoothVelocity;

    private float mouseX;
    private float mouseY;

    void Start()
    {
        //Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate() => FollowToTarget();
    private void LateUpdate() => RotateToMousePosition();

    private void FollowToTarget()
    {
        Vector3 movePosition = Vector3.SmoothDamp(transform.position, target.position,
            ref moveSmoothVelocity, moveSpeed);

        transform.position = movePosition;
    }
    private void RotateToMousePosition()
    {
        mouseY += -Input.GetAxis("Mouse Y") * rotationSpeed;
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY = Mathf.Clamp(mouseY, HORIZONTAL_MIN_ROTATION, HORIZONTAL_MAX_ROTATION);

        CamerLook();
        transform.rotation = Quaternion.Euler(mouseY, mouseX, 0f);
    }
    private void CamerLook()
    {
        Vector3 angle = (target.position - Camera.position).normalized;
        Vector3 smoothRotationAngle = Vector3.SmoothDamp(Camera.forward.normalized, angle, ref lookSmoothVelocity, lookSpeed);

        Camera.forward = smoothRotationAngle;
    }
}
