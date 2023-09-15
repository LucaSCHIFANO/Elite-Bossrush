using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Attack")]
public class Attack : ScriptableObject
{
    public bool IsComboEnd;
    public float GetDamage { get => CanBeSpecial && IsSpecial ? specialDamage : damage; }
    
    [HideInInspector] public bool IsSpecial { private get; set; }
    
    [SerializeField] private AnimationClip attackAnimationClip;
    
    [SerializeField] private float damage;

    [SerializeField] private bool CanBeSpecial;
    [SerializeField, ShowIf(nameof(CanBeSpecial))] private float specialDamage;

    public AnimatorOverrideController AnimationOverride { get; private set; }
    public float ClipLength { get => attackAnimationClip.length; }

    public void Initialize(RuntimeAnimatorController playerRuntimeAnimatorController)
    {
        // Create Override Controller from Player Animator and AnimationClip field
        AnimationOverride = new AnimatorOverrideController(playerRuntimeAnimatorController);
        AnimationOverride["Attack"] = attackAnimationClip;
    }

    public void Activate()
    {
        Debug.Log($"Hiyyaaa!  {{{GetDamage} dmg}}" + (IsSpecial ? " SPECIAL ATTACK !!!" : "."));
    }
}
