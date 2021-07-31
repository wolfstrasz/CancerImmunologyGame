using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ImmunotherapyGame
{
	[System.Serializable]
	public abstract class Cell : MonoBehaviour
	{
		[Header("GameObject Links")]
		[SerializeField]
		protected SpriteRenderer render = null;
		[SerializeField]
		protected Animator animator = null;
		[SerializeField]
		protected Collider2D bodyBlocker = null;

		[Header("Cell Attributes")]
		public CellType cellType;

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
		public float CurrentSpeed => Mathf.Clamp(speed, 0f, speed);

		public int RenderSortOrder { get => render.sortingOrder; set => render.sortingOrder = value; }
		public virtual bool isImmune { get; }

		protected virtual void Start()
		{
			Debug.Log("S1 HEALTH = " + health);

			health = cellType.MaxHealth;
			energy = cellType.MaxEnergy;
			speed = cellType.InitialSpeed;
			Debug.Log("s2 HEALTH = " + health);

			updateHealthValue = cellType.MaxHealth;
			updateEnergyValue = cellType.MaxEnergy;
			updateSpeedValue = 0;
			Debug.Log("s3 HEALTH = " + health);

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
					if (onDeathEvent != null)
						onDeathEvent(this);

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
			Debug.Log(this.gameObject.name + " got health applied: " + amount);
			updateHealthValue += amount;
		}

		public virtual void ApplyEnergyAmount(float amount)
		{
			Debug.Log(this.gameObject.name + " got energy applied: " + amount);
			updateEnergyValue += amount;
		}

		public virtual void ApplySpeedAmount(float amount)
		{
			Debug.Log(this.gameObject.name + " got speed for: " + amount);
			updateSpeedValue += amount;
		}

		protected virtual void OnCellDeath()
		{
			isDying = true;
		}

	}



}