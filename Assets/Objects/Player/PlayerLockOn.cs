using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class PlayerLockOn : MonoBehaviour
{
    private bool isLock;

    [Header("Camera")]
    [SerializeField] private CinemachineStateDrivenCamera cameraState;
    [SerializeField] private CinemachineVirtualCamera targetCamera;
    private Animator anim;

    [Header("Lock")]
    [SerializeField] float checkRadius;
    [SerializeField] private LayerMask lockableMask;
    [SerializeField] float maxAngleCheck;
    [SerializeField] float maxAngleChangeTarget;
    private GameObject currentTarget;

    void Awake()
    {
        anim = cameraState.m_AnimatedTarget;
    }

    void Update()
    {
        
    }

    void LockableSphere()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRadius, lockableMask);
        float closestAngle = maxAngleCheck;
        GameObject closestTarget = null;

        foreach (Collider collider in hitColliders)
        {
           Vector3 dir = collider.transform.position - Camera.main.transform.position;
           dir.y = 0;

           float angle = Vector3.Angle(Camera.main.transform.forward, dir);

           if(angle < closestAngle)
           {
               closestAngle = angle;
               closestTarget = collider.gameObject;
           }
           
        }

        if (closestTarget != null)
        {
            anim.Play("TargetCamera");
            targetCamera.LookAt = closestTarget.transform;
            isLock = !isLock;
            currentTarget = closestTarget;
        }
        else Debug.Log("No target found");
    }

    void ChangeTarget(bool left)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRadius, lockableMask);
        float closestAngle = maxAngleChangeTarget;
        GameObject closestTarget = null;


        foreach (Collider collider in hitColliders)
        {
            Vector3 dir = collider.transform.position - Camera.main.transform.position;
            //dir.y = 0;

            float angle = Vector3.Angle(Camera.main.transform.forward, dir);

            if (collider.gameObject == currentTarget) { continue; }

            if (left)
            {
                if (angle < closestAngle && Camera.main.WorldToScreenPoint(collider.transform.position).x < Camera.main.WorldToScreenPoint(currentTarget.transform.position).x)
                {
                    closestAngle = angle;
                    closestTarget = collider.gameObject;
                }
            }
            else
            {
                if (angle < closestAngle && Camera.main.WorldToScreenPoint(collider.transform.position).x > Camera.main.WorldToScreenPoint(currentTarget.transform.position).x)
                {
                    closestAngle = angle;
                    closestTarget = collider.gameObject;
                }
            }
            

        }

        if (closestTarget != null)
        {
            anim.Play("TargetCamera");
            targetCamera.LookAt = closestTarget.transform;
            isLock = !isLock;
            currentTarget = closestTarget;
        }
        else Debug.Log("No target found");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }

    #region Input Detection

    public void LockInput(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        if (isLock)
        {
            anim.Play("ThirdPersonCamera");
            isLock = !isLock;
            currentTarget = null;
        }
        else LockableSphere();

    }

    public void ChangeTargetInput(InputAction.CallbackContext context)
    {
        if (!context.started || currentTarget == null) return;

        float movementInput = context.ReadValue<float>();

        if(movementInput > 0) { ChangeTarget(false); }
        else if (movementInput < 0) { ChangeTarget(true); }
    }

    #endregion
}
