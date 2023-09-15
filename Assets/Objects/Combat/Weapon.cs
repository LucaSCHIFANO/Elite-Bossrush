using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Weapon")]
public class Weapon : ScriptableObject
{
    public List<Attack> moveset;

    public void Initialize(RuntimeAnimatorController runtimePlayerAnimationController)
    {
        for (int i = 0; i < moveset.Count; i++)
            moveset[i].Initialize(runtimePlayerAnimationController);
    }

    public Attack GetAttackFromCombo(int comboIndex, AttackInputType inputType)
    {
        var attack = moveset[comboIndex];
        attack.IsSpecial = inputType == AttackInputType.Special;
        return attack;
    }
}
