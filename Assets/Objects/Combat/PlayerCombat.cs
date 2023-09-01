using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private float comboCounterResetCooldown;
    private Animator animator;

    private int comboCounter = 0;
    private float comboCounterTimer;
    private bool attackInputTriggered;
    private bool isAttacking;

    private void Start()
    {
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


    #region Inputs 

    public void BasicAttackInput()
    {
        attackInputTriggered = true;
    }

    #endregion
}
