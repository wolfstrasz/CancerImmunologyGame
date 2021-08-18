using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.Abilities
{
    public class PacProjectile : Projectile
    {
		[Header("Pac Attributes")]
		[SerializeField] private float initialSpeed = 1f;
        [SerializeField] private float secondStageSpeed = 1f;
		[SerializeField] [ReadOnly] private Cell hitCell = null;

		/* ABILITY EFFECT */
		protected override void OnEnable()
		{
			base.OnEnable();
			hitCell = null;
			speed = initialSpeed;
		}

		/* PROJECTILE */
		protected override void OnCollisionWithTarget(Cell cell)
		{
			// Mark the cell to kill and cancel collider
			hitCell = cell;
			coll.enabled = false;

			// Set new auto die on range data to match centre of object
			spawnPosition = transform.position;
			direction = cell.transform.position - transform.position;
			maxRangeToMove = direction.magnitude;

			// Update movement data
			speed = secondStageSpeed;
			Vector3.Normalize(direction);

			// Update animation
			render.flipY = direction.x <= 0;
			transform.right = direction;
			animator.SetTrigger("Eat");
		}

		protected override void OnOutOfRange()
		{
			if (hitCell != null )
			{
				projectileAbility.ApplyAbilityEffect(hitCell);
			}

			OnLifeEnded();
		}


	}
}
