using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame
{
    public class MatrixCell : Cell
    {
		public float timeToReach = 1f;
		public float timePassed = 0f;

		public Transform startPosition = null;
		public Transform endPosition = null;

		public override bool isImmune => isDying;

		private void Awake()
		{
			healthBar.MaxHealth = maxHealth;
		}

		private void Update()
		{
			if (timePassed <  timeToReach)
			{
				timePassed += Time.deltaTime;
				if (timePassed > timeToReach)
				{
					timePassed = timeToReach;
					//transform.parent = endPosition;
				}

				transform.position = Vector3.Lerp(startPosition.position, endPosition.position, timePassed / timePassed);
			}

	
			
		}

		public override void ExhaustCell(float amount)
		{
			Debug.LogWarning("Trying to exhaust Matrix Cell but it is not implemented");
		}

		public override void HitCell(float amount)
		{
			if (isImmune) return;
			health -= amount;
			healthBar.Health = health;

			if (health <= 0.0f)
			{
				animator.Play("Destroyed");
				// Make destruction sound
				isDying = true;

			}
			else if (health * 3f <= maxHealth)
			{
				animator.Play("AlmostDestroyed");
				// Make destruction sound
			}
			else if (health * 1.5f <= maxHealth)
			{
				animator.Play("Damaged");
				// Make destruction sound

			}

		}

		public void SetMatrixData(Transform targetPosition, Transform startPosition, int SortLayerID)
		{
			this.startPosition = startPosition;
			endPosition = targetPosition;
			//render.sortingOrder = SortLayerID + 1;
		}

		public void OnDestroyMatrixCell()
		{
			Destroy(gameObject);
		}
    }
}
