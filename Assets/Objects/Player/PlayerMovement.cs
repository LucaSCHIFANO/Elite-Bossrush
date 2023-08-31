using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;


public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Movement")]
    private Vector2 movementInput;
    [SerializeField] private float speed;
    [SerializeField] private float turnSmoothTime;
    private float turnSmoothVelocity;

    [Header("Camera")]
    [SerializeField] private Transform cam;
    [SerializeField] private CinemachineFreeLook freeLook;
    private Vector2 cameraMovementInput;
    [SerializeField] Vector2 cameraSpeed;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Movement();
        CameraMovement(); ;
    }


    void Movement()
    {
        Vector3 direction = new Vector3(movementInput.x, 0f, movementInput.y);
        Vector3 moveDir = Vector3.zero;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }
        rb.velocity = moveDir * speed ;
    }

    void CameraMovement()
    {
        freeLook.m_XAxis.Value += cameraMovementInput.x * cameraSpeed.x;
        freeLook.m_YAxis.Value += cameraMovementInput.y * cameraSpeed.y;
    }


    #region Input Detection

    public void MovementInput(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void CameraMovementInput(InputAction.CallbackContext context)
    {
        cameraMovementInput = context.ReadValue<Vector2>();
    }

    #endregion
}
