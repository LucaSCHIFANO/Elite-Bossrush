using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System.Linq;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float turnSmoothTime;
    private Vector2 movementInput;
    private float turnSmoothVelocity;
    private float actualSpeed;

    [Header("Camera")]
    [SerializeField] private Transform cam;
    [SerializeField] private CinemachineFreeLook freeLook;
    private Vector2 cameraMovementInput;
    [SerializeField] Vector2 cameraSpeed;

    [Header("Dodge")]
    [SerializeField] private float speedMultiplicator;
    private Vector3 dodgeDir;

    [SerializeField] private float dodgeDuration;
    private float ondodgeDurationTimer;

    [SerializeField] private float dodgeCoolDown;
    private float dodgeCoolDownTimer;

    [SerializeField] private Animator animator;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        actualSpeed = speed;
    }

    void FixedUpdate()
    {
        Movement();
        CameraMovement();
        ResetDodge();
    }


    void Movement()
    {


        Vector3 direction = new Vector3(movementInput.x, 0f, movementInput.y);
        Vector3 moveDir = Vector3.zero;

        if (ondodgeDurationTimer > 0)
        {
            moveDir = dodgeDir;

            float targetAngle = Mathf.Atan2(dodgeDir.x, dodgeDir.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

        }
        else if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            dodgeDir = moveDir;

        }
        

        rb.velocity = moveDir * actualSpeed;
    }

    void CameraMovement()
    {
        freeLook.m_XAxis.Value += cameraMovementInput.x * cameraSpeed.x;
        freeLook.m_YAxis.Value += cameraMovementInput.y * cameraSpeed.y;
    }

    void Dodge()
    {
        if (ondodgeDurationTimer > 0 || dodgeCoolDownTimer > 0) return;

        actualSpeed = speed * speedMultiplicator;
        ondodgeDurationTimer = dodgeDuration;
        dodgeCoolDownTimer = dodgeCoolDown;
        animator.Play("Dodge");
    }

    void ResetDodge()
    {
        ondodgeDurationTimer -= Time.deltaTime;
        dodgeCoolDownTimer -= Time.deltaTime;

        if (ondodgeDurationTimer < 0)
        {
            animator.Play("Idle");
            actualSpeed = speed;
        }
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

    public void DodgeInput(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        Dodge();
    }

    #endregion
}
