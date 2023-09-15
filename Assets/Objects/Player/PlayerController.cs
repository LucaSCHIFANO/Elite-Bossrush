using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private PlayerCombat playerCombat;
    private PlayerLockOn playerLockOn;
    private PlayerMovement playerMovement;

    [Header("Component")]
    [SerializeField] private Animator animator;
    [SerializeField] private CinemachineFreeLook freeLook;
    [SerializeField] private CinemachineVirtualCamera targetCamera;

    void Awake()
    {
        playerCombat = GetComponent<PlayerCombat>();
        playerLockOn = GetComponent<PlayerLockOn>();
        playerMovement = GetComponent<PlayerMovement>();

        playerMovement.Initialized(freeLook, animator);
        playerLockOn.Initialized(targetCamera);
        playerCombat.Initialized(animator, targetCamera);
    }

    #region Inputs Detection

    //Movement
    public void MovementInput(InputAction.CallbackContext context)
    {
        playerMovement.MovementInput(context.ReadValue<Vector2>());
    }

    public void CameraMovementInput(InputAction.CallbackContext context)
    {
        playerMovement.CameraMovementInput(context.ReadValue<Vector2>());
    }

    public void DodgeInput(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        playerMovement.DodgeInput();
    }

    // Lock On
    public void LockInput(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        playerLockOn.LockInput();

    }
    public void ChangeTargetInput(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        playerLockOn.ChangeTargetInput(context.ReadValue<float>());
    }

    //Combat
    public void BasicAttackInput(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        playerCombat.BasicAttackInput();
    }

    public void SpecialAttackInput(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        playerCombat.SpecialAttackInput();
    }

    public void ShootInput(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        playerCombat.ShootInput();
    }

    #endregion

}
