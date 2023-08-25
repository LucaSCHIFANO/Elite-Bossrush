using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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

    void Awake()
    {
        anim = cameraState.m_AnimatedTarget;
    }

    void Update()
    {
        
    }

    public void LockInput()
    {
        if (isLock)
        {
            anim.Play("ThirdPersonCamera");
            isLock = !isLock;
        }
        else LockableSphere();

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
        }
        else Debug.Log("No target found");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
