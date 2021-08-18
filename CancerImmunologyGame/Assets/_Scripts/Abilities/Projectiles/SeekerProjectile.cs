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


		/* ABILITY EFFECT */
		protected override void OnEnable()
		{
			base.OnEnable();
			lifeSpan = -1000f;
			coll.radius = initialColliderRadius;
		}

		internal override void OnFixedUpdate()
		{
			if (lifeSpan > 0)
			{
				lifeSpan -= Time.fixedDeltaTime;
				if (lifeSpan <= 0)
				{
					// Reset particle
					OnLifeEnded();
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
				spawnPosition = transform.position;

				// Apply stationary behaviour data that is active until it detects a targetable cell
				maxRangeToMove = projectileAbility.Range;
				lifeSpan = projectileAbility.ProjectileLifetime;
				coll.radius = maxRangeToMove;
				coll.enabled = true;
				Debug.Log(this + " Coll not enabled ");

			}
			else
			{
				Debug.Log(this + " Coll enabled ");

				OnLifeEnded();
			}
		}

		protected override void OnCollisionWithTarget(Cell cell)
		{
			Debug.Log(this + " Collided with target: " + cell.name);
			if (coll.radius != initialColliderRadius)
			{
				Debug.Log(this + " Collider radius != initialColliderRadius ");

				// Remove lifespan without destroying it
				lifeSpan = -1000f;

				// Apply missile behaviour
				direction = (cell.transform.position - transform.position).normalized;
				speed = chargeSpeed;
				coll.radius = initialColliderRadius;
				coll.enabled = true;
				seekingVisual.SetActive(true);
				// TODO: if second stage animation is added switch to it.
			}
			else if (coll.radius == initialColliderRadius)
			{
				Debug.Log(this + " Collider radius == initialCollider radius -> must apply effect ");

				projectileAbility.ApplyAbilityEffect(cell);
				speed = initialSpeed;
				// Reset particle
				OnLifeEnded();
			}
		}


		public override void Shoot(Vector3 _direction, ProjectileAbility _projectileAbility)
		{
			base.Shoot(_direction, _projectileAbility);
			seekingVisual.SetActive(false);
			initialColliderRadius = coll.radius;
			coll.enabled = false;

			Debug.Log(this + " SHOT dir: " + direction);
		}


	}
}