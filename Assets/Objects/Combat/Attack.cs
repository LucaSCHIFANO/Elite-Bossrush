using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Attack")]
public class Attack : ScriptableObject
{
    public AnimatorOverrideController animOV;
    public float damage;
    public bool ComboEnd;

    public void Activate()
    {
        Debug.Log($"Hiyyaaa!  {{{damage} dmg}}");
    }
}
