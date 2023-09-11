using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(menuName = "Combat/Gun")]
public class Gun : ScriptableObject
{
	public ProjectileType projectileType;

	[SerializeField ,ShowIf("projectileType", ProjectileType.Projectile)] private GameObject projectile;

	public enum ProjectileType
	{
		Projectile,
		Ray,
	}

	public GameObject GetProjectile()
	{
		if (projectileType == ProjectileType.Projectile) return projectile;
		else return null;
	}
}
