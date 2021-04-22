using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame
{
	public class RegulatoryParticle : MonoBehaviour
	{
		[Header("Regulatory Cell Particle Attributes")]
		public float energyDmg = -5.0f;

		// Spreading
		[SerializeField]
		protected Vector3 spreadPosition;

		// Targeting
		[SerializeField]
		private Vector3 targetPosition = Vector3.zero;

		// Moving
		private Vector3 directionVector = Vector3.zero;

		// Attributes
		[SerializeField]
		protected float lifeSpan = 0.0f;
		[SerializeField]
		protected float speed = 1.0f;
		[SerializeField]
		private float attackSpeed = 5.0f;

		// State 
		delegate void Action();
		private Action StateAction = null;

		[SerializeField]
		private CircleCollider2D coll = null;
		[SerializeField]
		private float hitRadius = 1.0f;
		[SerializeField]
		private float detectRadius = 1.0f;
		[SerializeField]
		private GameObject glow = null;

		void Update()
		{
			if (GlobalGameData.isGameplayPaused) return;
			OnUpdate();
		}

		public void OnUpdate()
		{
			StateAction();
		}

		public void Initialise(Vector3 _spreadPosition)
		{
			spreadPosition = _spreadPosition;
			directionVector = (_spreadPosition - transform.position).normalized;
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
			Vector3 jumpValue = directionVector * Time.deltaTime * speed;
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
			Vector3 jumpValue = directionVector * Time.deltaTime * attackSpeed;
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

		void OnTriggerEnter2D(Collider2D collider)
		{
			KillerCell cell = collider.GetComponent<KillerCell>();
			if (cell != null)
			{
				if (coll.radius == detectRadius)
				{
					StateAction = ReachTargetPosition;
					coll.radius = hitRadius;
					targetPosition = cell.gameObject.transform.position;
					directionVector = (targetPosition - transform.position).normalized;
					glow.SetActive(true);
				}
				else
				{
					cell.ExhaustCell(Mathf.Abs(energyDmg));
					StateAction = Dead;
					Destroy(gameObject);
				}
			}
		}

	}
}