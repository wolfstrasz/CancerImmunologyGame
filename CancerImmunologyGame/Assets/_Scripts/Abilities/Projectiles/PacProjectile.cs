using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame
{
    public class PacProjectile : Projectile
    {
		[Header("Pac Attributes")]
        [SerializeField]
        private float secondStageSpeed = 1f;
		private float initialSpeed = 1f;

        [Header("Debug")]
        [ReadOnly]
        private Cell hitCell = null;


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

		protected override void OnEndOfEffect()
		{
			// Reset to original
			speed = initialSpeed;
			hitCell = null;

			coll.enabled = false;
			render.enabled = false;
			Destroy(gameObject);
		}

		protected override void OnOutOfRange()
		{
			if (hitCell != null )
			{
				projectileAbility.ApplyAbilityEffect(hitCell);
			}

			OnEndOfEffect();
		}


	}
}
