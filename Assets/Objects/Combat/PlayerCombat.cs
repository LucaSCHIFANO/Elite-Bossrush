using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private float comboCounterResetCooldown;
    [SerializeField] private Animator animator;

    private int comboCounter = 0;
    private float comboCounterTimer;

    private void Update()
    {
        if(comboCounter != 0 && comboCounterTimer <= 0)
        {
            comboCounter = 0;
        }
        else if (comboCounterTimer > 0)
        {
            comboCounterTimer -= Time.deltaTime;
        }
    }

    public void BasicAttackInput(InputAction.CallbackContext context)
    {
        if (!context.started)
            return;

        TriggerAttack();
    }

    private void TriggerAttack()
    {
        var attack = weapon.GetAttackFromCombo(comboCounter);

        animator.runtimeAnimatorController = attack.animOV;
        animator.Play("Attack");

        attack.Activate();

        comboCounter += attack.ComboEnd ? 0 : 1;

        comboCounterTimer = comboCounterResetCooldown;
    }
}
