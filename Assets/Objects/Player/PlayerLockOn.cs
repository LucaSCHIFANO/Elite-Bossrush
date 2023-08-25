using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerLockOn : MonoBehaviour
{
    private bool isLock;

    [SerializeField] private CinemachineStateDrivenCamera cameraState;
    private Animator anim;

    void Awake()
    {
        anim = cameraState.m_AnimatedTarget;
    }

    void Update()
    {
        
    }

    public void LockInput()
    {
        if (isLock) anim.Play("ThirdPersonCamera");
        else anim.Play("TargetCamera");

        isLock = !isLock;
    }
}
