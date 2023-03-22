using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    private const float HORIZONTAL_MIN_ROTATION = -30f;
    private const float HORIZONTAL_MAX_ROTATION = 60f;

    [SerializeField] private Transform Camera;
    [SerializeField] private Transform cameraPivot;
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
    private void LateUpdate()
    {
        FollowToTarget();
        RotateToMousePosition();
    }
    private void FollowToTarget()
    {
        Vector3 targetPosition = 
            Vector3.SmoothDamp(transform.position, target.position, ref moveSmoothVelocity, moveSpeed);

        transform.position = targetPosition;
    }
    private void RotateToMousePosition()
    {
        mouseY += -Input.GetAxis("Mouse Y") * rotationSpeed;
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY = Mathf.Clamp(mouseY, HORIZONTAL_MIN_ROTATION, HORIZONTAL_MAX_ROTATION);

        //CamerLook();
        Vector3 rotation = Vector3.zero;
        rotation.y = mouseX;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = mouseY;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }
    private void CamerLook()
    {
        Vector3 angle = (target.position - Camera.position).normalized;
        Vector3 smoothRotationAngle =
            Vector3.Slerp(Camera.forward.normalized, angle, lookSpeed * Time.deltaTime);

        Camera.forward = smoothRotationAngle;
    }
}
