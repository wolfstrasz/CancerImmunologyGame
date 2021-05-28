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

		public delegate void OnUpdateHealth();
		public OnUpdateHealth onUpdateHealth;

		public delegate void OnUpdateEnergy();
		public OnUpdateEnergy onUpdateEnergy;

		[ReadOnly]
		protected float health;
		[ReadOnly]
		protected float energy;
		[ReadOnly]
		protected bool isDying;

		public float Health => health;
		public float Energy => energy;
		public float Speed => speed;

		private float updateHealthValue = 0;
		private float updateEnergyValue = 0;

		public int RenderSortOrder { get => render.sortingOrder; set => render.sortingOrder = value; }
		public float maxHealth => cellType.MaxHealth;
		public float maxEnergy => cellType.MaxEnergy;
		public float speed => cellType.Speed;
		public virtual bool isImmune { get; }

		protected virtual void Start()
		{
			health = maxHealth;
			energy = maxEnergy;
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
					Mathf.Clamp(health, 0f, maxHealth);

					if (onUpdateHealth != null)
						onUpdateHealth();
				}

				if (updateEnergyValue != 0)
				{
					energy += updateEnergyValue;
					Mathf.Clamp(energy, 0f, maxEnergy);
					if (onUpdateEnergy != null)
						onUpdateEnergy();
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