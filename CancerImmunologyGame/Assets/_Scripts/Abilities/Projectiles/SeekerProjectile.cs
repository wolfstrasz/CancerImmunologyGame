using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ImmunotherapyGame.Abilities;

namespace ImmunotherapyGame
{
	public class SeekerProjectile : Projectile
	{
		[Header("Regulatory Cell Particle Attributes")]
		[SerializeField]
		private float chargeSpeed = 4f;

		[Header ("Debug")]
		[SerializeField]
		[ReadOnly]
		private float lifeSpan = 0.0f;
		[SerializeField]
		[ReadOnly]
		private float initialSpeed;
		[SerializeField]
		[ReadOnly]
		private float initialColliderRadius = 0f;

		public override void OnFixedUpdate()
		{
			if (lifeSpan > 0)
			{
				lifeSpan -= Time.fixedDeltaTime;
				if (lifeSpan <= 0)
				{
					// Reset particle
					OnEndOfEffect();
				}
			}

			base.OnFixedUpdate();
		}

		protected override void OnOutOfRange()
		{
			if (!coll.enabled)
			{
				// Set it to stay in one position and just scan for a target (detect radius collider)
				initialSpeed = speed;
				speed = 0f;
				spawnPosition = transform.position;

				// Apply stationary behaviour data that is active until it detects a targetable cell
				maxRangeToMove = projectileAbility.ProjectileRange;
				lifeSpan = projectileAbility.ProjectileLifetime;
				coll.radius = maxRangeToMove;
				coll.enabled = true;
			} else
			{
				OnEndOfEffect();
			}
		}

		protected override void OnCollisionWithTarget(Cell cell)
		{
			if (coll.radius != initialColliderRadius)
			{
				// Remove lifespan without destroying it
				lifeSpan = -1000f;

				// Apply missile behaviour
				direction = (cell.transform.position - transform.position).normalized;
				speed = chargeSpeed;
				coll.radius = initialColliderRadius;
				coll.enabled = true;

				// TODO: if second stage animation is added switch to it.
			}
			else if (coll.radius == initialColliderRadius)
			{
				projectileAbility.ApplyAbilityEffect(cell);

				// Reset particle
				OnEndOfEffect();
			}
		}

		protected override void OnEndOfEffect()
		{
			// Reset particle
			speed = initialSpeed;
			lifeSpan = -1000f;
			coll.radius = initialColliderRadius;
			coll.enabled = false;
			render.enabled = false;
			Destroy(gameObject);
		}

		public override void Shoot(Vector3 _direction, ProjectileAbility _projectileAbility)
		{
			base.Shoot(_direction, _projectileAbility);

			initialColliderRadius = coll.radius;
			coll.enabled = false;

		}


	}
}