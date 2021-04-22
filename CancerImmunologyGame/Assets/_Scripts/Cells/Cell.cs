using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		public float maxHealth;
		[SerializeField]
		protected float health;
		[SerializeField]
		public float maxEnergy;
		[SerializeField]
		protected float energy;
		[SerializeField]
		protected CellHealthBar healthBar;
		[SerializeField]
		protected bool isDying;


		public abstract void HitCell(float amount);
		public abstract void ExhaustCell(float amount);
		public abstract bool isImmune { get; }

		public int RenderSortOrder { set => render.sortingOrder = value; }

	}


	public abstract class EvilCell : Cell
	{
		[Header("Evil Cell")]

		[Header ("Debug (Evil Cell)")]
		[SerializeField]
		protected List<IEvilCellObserver> observers = new List<IEvilCellObserver>();


		public void AddObserver(IEvilCellObserver observer)
		{
			observers.Add(observer);
		}

		public void RemoveObserver(IEvilCellObserver observer)
		{
			observers.Remove(observer);
		}

		protected void NotifyObservers(EvilCell evilCell)
		{
			for (int i = 0; i < observers.Count; ++i)
			{
				observers[i].NotifyOfDeath(evilCell);
			}
		}

		public override void HitCell(float amount)
		{
			if (isImmune) return;

			health -= amount;
			healthBar.Health = health;

			if (health <= 0.0f)
			{
				isDying = true;
				healthBar.gameObject.SetActive(false);
				if (bodyBlocker != null)
					bodyBlocker.enabled = false;

				OnDeath();

				NotifyObservers(this);
			}
		}

		protected abstract void OnDeath();

		public void CellDied()
		{
			Destroy(gameObject);
		}


		public override void ExhaustCell(float amount)
		{
			Debug.LogWarning("Evil Cell Exhaust is not implemented but it is called!");
		}

	}

	public interface IEvilCellObserver
	{
		void NotifyOfDeath(EvilCell evilCell);
	}

	public abstract class GoodCell : Cell
	{

	}


}