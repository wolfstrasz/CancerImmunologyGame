using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ImmunotherapyGame.Abilities
{
	public class SeekerProjectile : Projectile
	{
		[Header("Seeker Attributes")]
		[SerializeField] private GameObject seekingVisual = null;
		[SerializeField] private float chargeSpeed = 4f;

		[Header ("Debug")]
		[SerializeField] [ReadOnly] private float lifeSpan = 0.0f;
		[SerializeField] [ReadOnly] private float initialSpeed;
		[SerializeField] [ReadOnly] private float initialColliderRadius = 0f;


		internal override void OnFixedUpdate()
		{
			if (lifeSpan > 0)
			{
				lifeSpan -= Time.fixedDeltaTime;
				if (lifeSpan <= 0)
				{
					ResetSeeekerParticle();
				}
			}

			base.OnFixedUpdate();
		}

		/* PROJECTILE */
		protected override void OnOutOfRange()
		{
			Debug.Log(this + " OutOfRange ");

			if (!coll.enabled)
			{
				// Set it to stay in one position and just scan for a target (detect radius collider)
				initialSpeed = speed;
				speed = 0f;

				// Update new range limits
				spawnPosition = transform.position;
				maxRangeToMove = projectileAbility.Range;

				// Apply stationary behaviour data that is active until it detects a targetable cell
				lifeSpan = projectileAbility.ProjectileLifetime;
				coll.radius = maxRangeToMove;
				coll.enabled = true;

			}
			else
			{
				ResetSeeekerParticle();
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
				seekingVisual.SetActive(true);
			}
			else if (coll.radius == initialColliderRadius)
			{
				projectileAbility.ApplyAbilityEffect(cell);
				ResetSeeekerParticle();
			}
		}


		public override void Shoot(Vector3 _direction, ProjectileAbility _projectileAbility)
		{
			base.Shoot(_direction, _projectileAbility);
			coll.enabled = false;
			initialColliderRadius = coll.radius;
			lifeSpan = -1000f;
			Debug.Log(this + " SHOT dir: " + direction);
		}

		private void ResetSeeekerParticle()
		{
			// Reset particle
			coll.radius = initialColliderRadius;
			speed = initialSpeed;
			seekingVisual.SetActive(false);
			OnLifeEnded();
		}
	}
}