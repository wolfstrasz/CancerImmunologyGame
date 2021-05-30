using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame
{
	public class RegulatoryParticle : CellParticle
	{
		[Header("Regulatory Cell Particle Attributes")]
		// Spreading
		[SerializeField]
		protected Vector3 spreadPosition;

		// Targeting
		[SerializeField]
		private Vector3 targetPosition = Vector3.zero;

		// Attributes
		[SerializeField]
		protected float lifeSpan = 0.0f;
		[SerializeField]
		private float attackSpeed = 5.0f;

		// State 
		delegate void Action();
		private Action StateAction = null;

		[SerializeField]
		private float hitRadius = 1.0f;
		[SerializeField]
		private float detectRadius = 1.0f;
		[SerializeField]
		private GameObject glow = null;

		void FixedUpdate()
		{
			if (GlobalGameData.isGameplayPaused) return;
			StateAction();
		}


		public void Initialise(Vector3 directionVector, float range)
		{
			spreadPosition = transform.position + directionVector * range;
			this.direction = directionVector;
			StateAction = Spread;
		}

		private void Idle()
		{
			lifeSpan -= Time.deltaTime;
			if (lifeSpan <= 0.0f)
			{
				StateAction = Dead;
				Destroy(gameObject);
			}
		}

		// State machine
		private void Spread()
		{
			Vector3 jumpValue = direction * Time.deltaTime * speed;
			if (Vector3.SqrMagnitude(jumpValue) < Vector3.SqrMagnitude(transform.position - spreadPosition))
			{
				transform.position = transform.position + jumpValue;
			}
			else
			{
				StateAction = Idle;
			}
		}

		private void ReachTargetPosition()
		{
			Vector3 jumpValue = direction * Time.deltaTime * attackSpeed;
			if (Vector3.SqrMagnitude(jumpValue) < Vector3.SqrMagnitude(transform.position - targetPosition))
			{
				transform.position = transform.position + jumpValue;
			}
			else
			{
				StateAction = Dead;
				Destroy(gameObject);
			}
		}

		private void Dead() { }

		protected override void OnCollisionWithTarget(Cell cell)
		{
			throw new System.NotImplementedException();
		}

		//void OnTriggerEnter2D(Collider2D collider)
		//{
		//	KillerCell cell = collider.GetComponent<KillerCell>();
		//	if (cell != null)
		//	{
		//		if (coll.radius == detectRadius)
		//		{
		//			StateAction = ReachTargetPosition;
		//			coll.radius = hitRadius;
		//			targetPosition = cell.gameObject.transform.position;
		//			direction = (targetPosition - transform.position).normalized;
		//			glow.SetActive(true);
		//		}
		//		else
		//		{
		//			cell.ExhaustCell(Mathf.Abs(effectToEnergy));
		//			StateAction = Dead;
		//			Destroy(gameObject);
		//		}
		//	}
		//}

	}
}