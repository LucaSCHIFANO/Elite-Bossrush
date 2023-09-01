using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private PlayerCombat playerCombat;
    private PlayerLockOn playerLockOn;
    private PlayerMovement playerMovement;

    [Header("Component")]
    [SerializeField] private Animator animator;
    [SerializeField] private CinemachineFreeLook freeLook;
    
    void Awake()
    {
        playerCombat = GetComponent<PlayerCombat>();
        playerLockOn = GetComponent<PlayerLockOn>();
        playerMovement = GetComponent<PlayerMovement>();

        playerMovement.Initialized(freeLook, animator);
        playerCombat.Initialized(animator);
    }

    void Update()
    {
        
    }
}
