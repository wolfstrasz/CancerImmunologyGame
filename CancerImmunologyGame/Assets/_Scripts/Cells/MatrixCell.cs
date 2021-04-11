using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cells
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
				isDying = true;
				Destroy(gameObject);
			}
		}

		public void SetMatrixData(Transform targetPosition, Transform startPosition)
		{
			this.startPosition = startPosition;
			endPosition = targetPosition;
		}
    }
}
