using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.Cancers;

namespace ImmunotherapyGame
{
    public class MatrixCell : Cell
    {
		[SerializeField] [ReadOnly] CancerCell cancerCell = null;

		public override bool isImmune => isDying;

		protected override void LateUpdate()
		{
			base.LateUpdate();

			if (CurrentHealth == 0)
				return;

			if (CurrentHealth * 3f <= cellType.MaxHealth)
			{
				animator.Play("AlmostDestroyed");
				// Make destruction sound
			}
			else if (CurrentHealth * 1.5f <= cellType.MaxHealth)
			{
				animator.Play("Damaged");
				// Make destruction sound
			}
		}

		protected override void OnCellDeath()
		{
			animator.Play("Destroyed");
			// Make destruction sound
			cancerCell.DetachMatrixCell(this);
		}

		public void AttachCancerCell (CancerCell cancerCell)
		{
			this.cancerCell = cancerCell;
			RenderSortOrder = cancerCell.RenderSortOrder + 1;
		}

		public void DetachCancerCell (CancerCell cancerCell)
		{
			if (this.cancerCell == cancerCell)
			{
				cancerCell = null;
				ApplyHealthAmount(-CurrentHealth);
			}
		}
	}
}
