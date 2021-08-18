using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.Abilities
{  
	public class SimpleProjectile : Projectile
	{
		/* PROJECTILE */
		protected override void OnOutOfRange()
		{
			OnLifeEnded();
		}

		protected override void OnCollisionWithTarget(Cell cell)
		{
			projectileAbility.ApplyAbilityEffect(cell);
			OnLifeEnded();
		}
	}
}