using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Attack")]
public class Attack : ScriptableObject
{
    public float damage;
    public bool ComboEnd;

    [SerializeField] private AnimationClip attackAnimationClip;

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
        Debug.Log($"Hiyyaaa!  {{{damage} dmg}}");
    }
}
