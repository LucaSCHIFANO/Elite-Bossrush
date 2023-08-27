using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Weapon")]
public class Weapon : ScriptableObject
{
    public List<Attack> moveset;

    public Attack GetAttackFromCombo(int comboIndex)
    {
        return moveset[comboIndex];
    }
}
