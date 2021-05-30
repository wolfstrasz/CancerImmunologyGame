using UnityEngine;
using ImmunotherapyGame.Core;

namespace ImmunotherapyGame
{
	public abstract class CellParticle : MonoBehaviour
	{
		protected Animator animator = null;
		protected SpriteRenderer render = null;
		protected Collider2D coll = null;

		[SerializeField]
		protected float speed = 1.0f;
		[SerializeField]
		protected float effectToHealth = 2.0f;
		[SerializeField]
		protected float effectToEnergy = 2.0f;
		[SerializeField]
		protected float range;
		[SerializeField]
		protected float lifetime = 0;

		[ReadOnly]
		protected Vector3 spawnPosition;
		[ReadOnly]
		protected Vector3 direction;
		[ReadOnly]
		protected float rangeSq;

		protected virtual void Start()
		{
			animator = GetComponent<Animator>();
			render = GetComponent<SpriteRenderer>();
			coll = GetComponent<Collider2D>();
		}

		protected virtual void FixedUpdate()
		{
			OnFixedUpdate();
		}


		protected virtual void OnFixedUpdate()
		{
			if (Vector3.SqrMagnitude(transform.position - spawnPosition) > rangeSq)
			{
				DestroyParticle();
				return;
			}

			transform.position += direction * speed * Time.fixedDeltaTime;
		}


		public virtual void Shoot(Vector3 _direction, float _range)
		{
			spawnPosition = transform.position;

			range = _range;
			rangeSq = _range * _range;

			direction = _direction;
			render.flipY = _direction.x <= 0;
			transform.right = _direction;
		}

		protected virtual void DestroyParticle()
		{
			coll.enabled = false;
			render.enabled = false;
			Destroy(gameObject);
		}


		protected virtual void OnTriggerEnter2D(Collider2D collider)
		{

			Cell cell = collider.gameObject.GetComponent<Cell>();
			if (cell)
			{
				// for (int i = 0; i < targetCellTypes; ++i)
				// {
				//		if (targetCellTypes[i] == cell.cellType)
				//		{
				//			OnCollisionWithTarget(cell);
				//			break;
				//		}
				// }
			}
		}

		protected abstract void OnCollisionWithTarget(Cell cell);

	}
}