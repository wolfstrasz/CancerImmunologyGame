using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ImmunotherapyGame.Core;
namespace ImmunotherapyGame.Cancers
{
	public class CancerCell : Cell
	{
		[Header("Links")]
		[SerializeField]
		public CircleCollider2D divisionBodyBlocker = null;
		[SerializeField]
		private GameObject hypoxicArea = null;
		[SerializeField]
		internal Cancer cancerOwner = null;

		[Header("Debug (Read Only)")]
		[SerializeField]
		private float rotationAngle = 0.0f;
		[SerializeField]
		private bool isInDivision = false;

		public override bool isImmune => isDying || isInDivision || matrix != null;


		// Matrix handling
		// -------------------------------------------------
		[ReadOnly]
		public MatrixCell matrix = null;


		void Awake()
		{
			hypoxicArea.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f));
			healthBar.owner = this;

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
			healthBar.gameObject.SetActive(false);
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
			healthBar.gameObject.SetActive(true);
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
			if (!hypoxicArea.activeSelf)
			{
				float randomAngle = Random.Range(0.0f, 360.0f);
				hypoxicArea.transform.rotation = Quaternion.Euler(0, 0, randomAngle);
				hypoxicArea.SetActive(true);

				//   UIManager.Instance.allCancerCells.Add(this);
			}
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
		}

	}
}