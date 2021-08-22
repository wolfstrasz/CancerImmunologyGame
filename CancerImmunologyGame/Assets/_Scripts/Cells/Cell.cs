using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ImmunotherapyGame
{
	[System.Serializable]
	public abstract class Cell : MonoBehaviour
	{
		[Header("GameObject Links")]
		[SerializeField] protected SpriteRenderer render = null;
		[SerializeField] protected Animator animator = null;
		[SerializeField] protected Collider2D bodyBlocker = null;

		[Header("Cell Attributes")]
		[SerializeField] protected CellType cellType;

		[SerializeField] [ReadOnly] protected bool isDying;
		[SerializeField] [ReadOnly] private float health;
		[SerializeField] [ReadOnly] private float energy;
		[SerializeField] [ReadOnly] private float speed;
		[SerializeField] [ReadOnly] private float updateHealthValue;
		[SerializeField] [ReadOnly] private float updateEnergyValue;
		[SerializeField] [ReadOnly] private float updateSpeedValue;

		public delegate void OnDeathEvent(Cell cell);
		public OnDeathEvent onDeathEvent;

		public delegate void OnUpdateHealth();
		public OnUpdateHealth onUpdateHealth;

		public delegate void OnUpdateEnergy();
		public OnUpdateEnergy onUpdateEnergy;

		public delegate void OnUpdateSpeed();
		public OnUpdateSpeed onUpdateSpeed;

		public float CurrentHealth => Mathf.Clamp(health, 0f, cellType.MaxHealth);
		public float CurrentEnergy => Mathf.Clamp(energy, 0f, cellType.MaxEnergy);
		public float CurrentSpeed => Mathf.Clamp(speed + cellType.InitialSpeed, 0f, speed + cellType.InitialSpeed);

		public float CurrentHealthPercentage => Mathf.Clamp(health / cellType.MaxHealth, 0f, 1f);
		public float CurrentEnergyPercentage => Mathf.Clamp(energy / cellType.MaxEnergy, 0f, 1f);

		public float MaxHealth => cellType.MaxHealth;
		public float MaxEnergy => cellType.MaxEnergy;

		public CellType CellType => cellType; 

		public int RenderSortOrder { get => render.sortingOrder; set => render.sortingOrder = value; }
		public virtual bool isImmune { get; }

		protected virtual void Start()
		{
			health = cellType.MaxHealth;
			energy = cellType.MaxEnergy;
			speed = 0;

			updateHealthValue = cellType.MaxHealth;
			updateEnergyValue = cellType.MaxEnergy;
			updateSpeedValue = 0;

		}

		public virtual void OnUpdate()
		{
			updateEnergyValue += cellType.EnergyRegenPerSecond * Time.deltaTime;
			updateHealthValue += cellType.HealthRegenPerSecond * Time.deltaTime;
		}

		protected virtual void LateUpdate()
		{
			if (updateSpeedValue != 0)
			{
				speed += updateSpeedValue;
				if (onUpdateSpeed != null)
					onUpdateSpeed();
			}

			if (!isImmune)
			{
				if (updateHealthValue != 0)
				{
					health += updateHealthValue;
					health = Mathf.Clamp(health, 0f, cellType.MaxHealth);
					if (onUpdateHealth != null)
						onUpdateHealth();
				}

				if (updateEnergyValue != 0)
				{
					energy += updateEnergyValue;
					energy = Mathf.Clamp(energy, 0f, cellType.MaxEnergy);
					if (onUpdateEnergy != null)
						onUpdateEnergy();
				}

				if (health <= 0 || energy <= 0)
				{
					OnCellDeath();
				}
			}


			updateHealthValue = 0f;
			updateEnergyValue = 0f;
			updateSpeedValue = 0f;
		}

		// Attribute changes
		public virtual void ApplyHealthAmount(float amount)
		{
			//Debug.Log(this.gameObject.name + " got health applied: " + amount);
			updateHealthValue += amount;
		}

		public virtual void ApplyEnergyAmount(float amount)
		{
			//Debug.Log(this.gameObject.name + " got energy applied: " + amount);
			updateEnergyValue += amount;
		}

		public virtual void ApplySpeedAmount(float amount)
		{
			//Debug.Log(this.gameObject.name + " got speed for: " + amount);
			updateSpeedValue += amount;
		}

		public void Respawn()
		{
			isDying = false;
			health = cellType.MaxHealth;
			energy = cellType.MaxEnergy;
		}

		/// <summary>
		/// Method called when the cell's health or energy reaches 0.
		/// </summary>
		protected abstract void OnCellDeath();

	}



}