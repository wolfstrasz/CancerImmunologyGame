﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cancers
{
	public class CancerCell : MonoBehaviour
	{
		[Header("Links")]
		[SerializeField]
		private CancerCellBody body = null;
		[SerializeField]
		public CircleCollider2D divisionBodyBlocker = null;
		[SerializeField]
		private GameObject hypoxicArea = null;
		[SerializeField]
		private Animator animator = null;
		[SerializeField]
		private SpriteRenderer render = null;
		[SerializeField]
		private CellHealthBar healthbar = null;

		[Header("Attributes")]
		[SerializeField]
		private float maxHealth = 100.0f;
		[SerializeField]
		private float health = 100;

		[Header("Debug (Read Only)")]
		[SerializeField]
		internal Cancer cancer = null;
		[SerializeField]
		private float rotationAngle = 0.0f;
		[SerializeField]
		private bool inDivision = false;
		[SerializeField]
		internal bool isDying = false;

		public bool InDivision => inDivision;
		/// <summary>
		/// Sets the rendering sort order of the Cancer cell sprite
		/// </summary>
		internal int RenderSortOrder { set => render.sortingOrder = value; }


		void Awake()
		{
			hypoxicArea.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f));
			healthbar.MaxHealth = maxHealth;
			healthbar.Health = health;
		}


		public bool HitCell()
		{
			if (isDying || inDivision) return false;

			health -= 20f;
			if (health <= 0)
			{
				isDying = true;

				healthbar.gameObject.SetActive(false);
				if (cancer != null)
				{
					cancer.RemoveCell(this);
				}
				body.gameObject.SetActive(false);
				animator.SetTrigger("Apoptosis");
				return true;
			}

			healthbar.Health = health;
			return false;
		}

		// Cancer cell division animation flow functions
		// Control calls
		internal void StartPrepareDivision(float _rotationAngle)
		{
			inDivision = true;

			divisionBodyBlocker.gameObject.SetActive(true);
			animator.SetTrigger("PrepareToDivide");
			rotationAngle = _rotationAngle;
		}


		internal void StartDivision()
		{
			healthbar.gameObject.SetActive(false);
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
			cancer.OnFinishDivisionPreparation();
		}

		/// <summary>
		/// Callback to use in division animation.
		/// </summary>
		public void FinishedDivision()
		{
			healthbar.gameObject.SetActive(true);
			divisionBodyBlocker.gameObject.SetActive(false);
			cancer.OnFinishDivision();
		}

		/// <summary>
		/// Callback to use in division animation.
		/// </summary>
		public void CellSpawned()
		{
			inDivision = false;
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
		public void CellDied()
		{
			Destroy(gameObject);
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
	}
}