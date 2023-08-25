using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerLockOn : MonoBehaviour
{
    private bool isLock;

    [Header("Camera")]
    [SerializeField] private CinemachineStateDrivenCamera cameraState;
    private Animator anim;

    [Header("Lock")]
    [SerializeField] float checkRadius;
    [SerializeField] private LayerMask lockableMask;

    void Awake()
    {
        anim = cameraState.m_AnimatedTarget;
    }

    void Update()
    {
        
    }

    public void LockInput()
    {
        /*if (isLock) anim.Play("ThirdPersonCamera");
        else anim.Play("TargetCamera");

        isLock = !isLock;*/

        LockableSphere();
    }

    void LockableSphere()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRadius, lockableMask);
        foreach (Collider collider in hitColliders)
        {
            Debug.Log(collider.name);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
