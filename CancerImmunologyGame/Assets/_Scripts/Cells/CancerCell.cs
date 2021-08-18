using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ImmunotherapyGame.Core;
using ImmunotherapyGame.Abilities;

namespace ImmunotherapyGame.Cancers
{
	public class CancerCell : Cell
	{
		[Header("Links")]
		[SerializeField]
		public CircleCollider2D divisionBodyBlocker = null;
		[SerializeField]
		private RangedAbilityCaster hypoxicCaster = null;
		[SerializeField]
		internal Cancer cancerOwner = null;

		[Header("Debug (Read Only)")]
		[SerializeField]
		private float rotationAngle = 0.0f;
		[SerializeField]
		private bool isInDivision = false;

		public override bool isImmune => isDying || isInDivision || matrixCell != null;

		// Matrix handling
		// -------------------------------------------------
		[ReadOnly]
		public MatrixCell matrixCell = null;

		private void Awake()
		{
			if (hypoxicCaster != null)
			{
				hypoxicCaster.CastAbility(gameObject);
			}
		}

		// DIVISION HANDLERS
		// -----------------------------------------------------------------
		internal void StartPrepareDivision(float _rotationAngle)
		{
			isInDivision = true;

			divisionBodyBlocker.gameObject.SetActive(true);
			animator.SetTrigger("PrepareToDivide");
			rotationAngle = _rotationAngle;
		}

		internal void StartDivision()
		{
			animator.SetTrigger("Divide");
		}

		internal void StartReturnFromDivision()
		{
			animator.SetTrigger("ReturnFromDivision");
		}

		/// <summary>
		/// Callback to use in division animation.
		/// </summary>
		public void FinishedDivisionPreparation()
		{
			cancerOwner.OnFinishDivisionPreparation();
		}

		/// <summary>
		/// Callback to use in division animation.
		/// </summary>
		public void FinishedDivision()
		{
			divisionBodyBlocker.gameObject.SetActive(false);
			cancerOwner.OnFinishDivision();
		}

		/// <summary>
		/// Callback to use in division animation.
		/// </summary>
		public void CellSpawned()
		{
			isInDivision = false;
			isDying = false;
		}

		/// <summary>
		/// Callback to use in division animation.
		/// </summary>
		public void RotateForDivision()
		{
			transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationAngle);
		}

		/// <summary>
		/// Callback to use in division animation.
		/// </summary>
		public void RotateForReturn()
		{
			transform.rotation = Quaternion.identity;
		}


		protected override void OnCellDeath()
		{
			isDying = true;
			animator.SetTrigger("Apoptosis");
			divisionBodyBlocker.enabled = false;
			bodyBlocker.enabled = false;

			if (matrixCell != null)
			{
				matrixCell.DetachCancerCell(this);
			}
		}

		public void AttachMatrixCell(MatrixCell matrixCell)
		{
			this.matrixCell = matrixCell;
		}

		public void DetachMatrixCell(MatrixCell matrixCell)
		{
			if (this.matrixCell == matrixCell)
			{
				this.matrixCell = null;
			}
		}

		public void OnApoptosisEnd()
		{
			if (onDeathEvent != null)
				onDeathEvent(this);

			Destroy(gameObject);
		}
	}
}