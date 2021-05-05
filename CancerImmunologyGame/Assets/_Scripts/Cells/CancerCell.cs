using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.Cancers
{
	public class CancerCell : EvilCell
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

		public override bool isImmune => isDying || isInDivision;

		
		

		void Awake()
		{
			hypoxicArea.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f));
			healthBar.MaxHealth = maxHealth;
			healthBar.Health = health;
		}

		protected override void OnDeath()
		{
			animator.SetTrigger("Apoptosis");
			divisionBodyBlocker.enabled = false;
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



		// Matrix handling
		// -------------------------------------------------
		[Header("Debug(MatrixCell)")]
		[SerializeField]
		public MatrixCell matrix = null;
		[SerializeField]
		public Transform matrixAttachmentPoint = null; 



		public override void HitCell(float amount)
		{
			if (isImmune) return;

			if (matrix == null)
			{
				health -= amount;
				healthBar.Health = health;
				if (health <= 0.0f)
				{
					isDying = true;
					healthBar.gameObject.SetActive(false);
					bodyBlocker.enabled = false;

					OnDeath();

					NotifyObservers(this);
				}
			}
		}
	}
}