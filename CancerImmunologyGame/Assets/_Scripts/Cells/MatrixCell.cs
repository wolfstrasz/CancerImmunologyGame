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

		public override bool isImmune => isDying || !bodyBlocker.enabled;


		private void Awake()
		{
			bodyBlocker.enabled = false;
			healthBar.owner = this;
		}

		private void FixedUpdate()
		{
			if (timePassed < timeToReach)
			{
				timePassed += Time.fixedDeltaTime;
				if (timePassed > timeToReach)
				{
					timePassed = timeToReach;
					bodyBlocker.enabled = true;
				}

				transform.position = Vector3.Lerp(startPosition.position, endPosition.position, timePassed / timePassed);
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


		protected override void LateUpdate()
		{
			base.LateUpdate();

			if (health == 0)
				return;

			if (health * 3f <= cellType.maxHealthValue)
			{
				animator.Play("AlmostDestroyed");
				// Make destruction sound
			}
			else if (health * 1.5f <= cellType.maxHealthValue)
			{
				animator.Play("Damaged");
				// Make destruction sound
			}
		}

		protected override void OnCellDeath()
		{
			animator.Play("Destroyed");
			// Make destruction sound
		}

	}
}
