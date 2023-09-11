using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Gun")]
public class Gun : ScriptableObject
{
	public ProjectileType projectileType;

	//[SerializeField,show]

	public enum ProjectileType
	{
		Projectile,
		Ray,
	}
}
