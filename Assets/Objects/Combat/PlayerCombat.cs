using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    private Camera mainCamera;

    [Header("Melee Weapon")]
    [SerializeField] private Weapon weapon;
    [SerializeField] private float comboCounterResetCooldown;
    private Animator animator;

    private int comboCounter = 0;
    private float comboCounterTimer;
    private bool attackInputTriggered;
    private bool isAttacking;

    [Header("Range Weapon")]
    [SerializeField] private Gun gun;
    [SerializeField] private float raycastShootThickness;
    [SerializeField] private float maxDistanceCheck;
    private CinemachineVirtualCamera targetCamera;

    private bool shootInputTriggered;


    private void Start()
    {
        mainCamera = Camera.main;
    }

    public void Initialized(Animator _animator, CinemachineVirtualCamera _targetCamera)
    {
        animator = _animator;
        weapon?.Initialize(animator.runtimeAnimatorController);

        targetCamera = _targetCamera;
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

        // Trigger Shoot if input buffered
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
        var targetedPoint = GetProjectileDirection();
        var direction = (targetedPoint - transform.position).normalized;
        if(gun.projectileType == Gun.ProjectileType.Projectile)
        {
            Instantiate(gun.GetProjectile(), transform.position, transform.rotation).GetComponent<Projectile>().Initialized(direction);
        }
    }

    private Vector3 GetProjectileDirection()
    {
        if (targetCamera.m_LookAt != null) return targetCamera.LookAt.position;

        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;
        //if(Physics.SphereCast(ray, raycastShootThickness, out hit, maxDistanceCheck))
        if(Physics.Raycast(ray, out hit, maxDistanceCheck))
        {
            return hit.point;
        }
        else
        {
            Debug.Log("nothing to shoot at");
            return ray.GetPoint(maxDistanceCheck);

        }
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
