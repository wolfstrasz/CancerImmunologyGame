using UnityEngine;
using System.Collections.Generic;


namespace ImmunotherapyGame.Abilities
{
	public abstract class Projectile : AbilityEffect
	{
		[Header("Linking")]
		[SerializeField] protected Animator animator = null;
		[SerializeField] protected SpriteRenderer render = null;
		[SerializeField] protected CircleCollider2D coll = null;

		[Header("Attributes")]
		[SerializeField] protected float speed = 1.0f;
		[SerializeField] [ReadOnly] protected ProjectileAbility projectileAbility = null;

		[ReadOnly] protected Vector3 spawnPosition;
		[ReadOnly] protected Vector3 direction;
		[ReadOnly] protected float maxRangeToMove = 0f;


		/* ABILITY EFFECT */
		protected virtual void Awake() 
		{
			coll.isTrigger = true;
		}

		internal override void OnFixedUpdate()
		{
			// Move projectile
			transform.position += direction * speed * Time.fixedDeltaTime;

			// Check for out of range
			if (Vector3.Magnitude(transform.position - spawnPosition) > maxRangeToMove)
			{
				OnOutOfRange();
				return;
			}

		}

		/* PROJECTILE */
		protected virtual void OnTriggerEnter2D(Collider2D collider)
		{
			Cell cell = collider.gameObject.GetComponent<Cell>();
			if (cell)
			{
				if (projectileAbility.CanHitCellType(cell.CellType))
				{
					OnCollisionWithTarget(cell);
				}
			}
		}

		protected abstract void OnOutOfRange();
		protected abstract void OnCollisionWithTarget(Cell cell);

		public virtual void Shoot(Vector3 _direction, ProjectileAbility _projectileAbility)
		{
			direction = _direction;
			projectileAbility = _projectileAbility;

			spawnPosition = transform.position;
			maxRangeToMove = _projectileAbility.Range;

			render.flipY = _direction.x <= 0;
			transform.right = _direction;

			coll.enabled = true;
		}

	}
}