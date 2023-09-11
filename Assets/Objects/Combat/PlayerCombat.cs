using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    private Camera mainCamera;

    [SerializeField] private Weapon weapon;
    [SerializeField] private float comboCounterResetCooldown;
    private Animator animator;

    private int comboCounter = 0;
    private float comboCounterTimer;
    private bool attackInputTriggered;
    private bool isAttacking;

    [SerializeField] private Gun gun;
    private bool shootInputTriggered;


    private void Start()
    {
        mainCamera = Camera.main;
    }

    public void Initialized(Animator _animator)
    {
        animator = _animator;
        weapon?.Initialize(animator.runtimeAnimatorController);
    }

    private void Update()
    {
        // Decrease Combo window Timer
        if (comboCounterTimer > 0)
            comboCounterTimer -= Time.deltaTime;

        if (isAttacking)
            return;

        // End Combo if Combo window Expired
        if(comboCounter != 0 && comboCounterTimer <= 0)
            EndCombo();
        
        // Trigger Attack if input buffered
        if (attackInputTriggered)
            TriggerAttack();

        if (shootInputTriggered)
            TriggerShoot();
    }

    private void TriggerAttack()
    {
        var attack = weapon.GetAttackFromCombo(comboCounter);

        animator.runtimeAnimatorController = attack.AnimationOverride;
        animator.Play("Attack");
        attack.Activate();

        if (attack.ComboEnd)
            Invoke(nameof(EndCombo), attack.ClipLength);
        else
        {
            Invoke(nameof(EndAttack), attack.ClipLength);
            comboCounter++;
        }

        comboCounterTimer = comboCounterResetCooldown;
        isAttacking = true;
        attackInputTriggered = false;
    }

    private void EndAttack()
    {
        isAttacking = false;
    }

    private void EndCombo()
    {
        EndAttack();
        comboCounter = 0;
    }

    private void TriggerShoot()
    {
        shootInputTriggered = false;
        GetProjectileDirection();
    }

    private Vector3 GetProjectileDirection()
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1000))
        {
            Debug.Log(hit.collider.name);
        }
        else
        {
            Debug.Log("Nothing to hit");

        }
        return Vector3.back;
    }


    #region Inputs 

    public void BasicAttackInput()
    {
        attackInputTriggered = true;
    }

    public void ShootInput()
    {
        shootInputTriggered = true;
    }

    #endregion
}
