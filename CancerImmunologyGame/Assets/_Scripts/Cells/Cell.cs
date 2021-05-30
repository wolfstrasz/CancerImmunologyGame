using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ImmunotherapyGame.Core;

namespace ImmunotherapyGame
{

	public abstract class Cell : MonoBehaviour
	{
		[Header("Cell Links")]
		[SerializeField]
		protected SpriteRenderer render = null;
		[SerializeField]
		protected Animator animator = null;
		[SerializeField]
		protected Collider2D bodyBlocker = null;

		[Header("Cell Attributes")]
		[SerializeField]
		public CellType cellType;

		[SerializeField]
		protected CellHealthBar healthBar;

		public delegate void OnDeathEvent(Cell cell);
		public OnDeathEvent onDeathEvent;

		public delegate void OnUpdateHealth(float newValue);
		public OnUpdateHealth onUpdateHealth;

		public delegate void OnUpdateEnergy(float newValue);
		public OnUpdateEnergy onUpdateEnergy;

		[ReadOnly]
		protected float health;
		[ReadOnly]
		protected float energy;
		[ReadOnly]
		protected bool isDying;


		private float updateHealthValue = 0;
		private float updateEnergyValue = 0;

		public int RenderSortOrder { get => render.sortingOrder; set => render.sortingOrder = value; }
		public virtual bool isImmune { get; }

		protected virtual void Start()
		{
			health = 0;
			energy = 0;
			updateHealthValue += cellType.maxHealthValue;
			updateEnergyValue += cellType.maxEnergyValue;
		}

		public virtual void ApplyHealthAmount(float amount)
		{
			updateHealthValue += amount;
		}

		public virtual void ApplyEnergyAmount(float amount) 
		{
			updateEnergyValue += amount;
		}

		protected abstract void OnCellDeath();

		protected virtual void LateUpdate()
		{
			if (!isImmune)
			{

				if (updateHealthValue != 0)
				{
					health += updateHealthValue;
					Mathf.Clamp(health, 0f, cellType.maxHealthValue);

					if (onUpdateHealth != null)
						onUpdateHealth(health);
				}

				if (updateEnergyValue != 0)
				{
					energy += updateEnergyValue;
					Mathf.Clamp(energy, 0f, cellType.maxEnergyValue);
					if (onUpdateEnergy != null)
						onUpdateEnergy(energy);
				}
			}

			updateHealthValue = 0f;
			updateEnergyValue = 0f;

			if (health <= 0 || energy <= 0)
			{
				if (onDeathEvent != null)
					onDeathEvent(this);

				OnCellDeath();
			}
		}


	}

	

}