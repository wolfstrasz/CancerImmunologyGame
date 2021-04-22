using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ImmunotherapyGame
{
	[RequireComponent(typeof(CircleCollider2D))]
	public class KillerSense : MonoBehaviour, IEvilCellObserver
	{
		[SerializeField]
		private KillerCell owner = null;
		[SerializeField]
		public ICellController controller = null;

		[SerializeField]
		private CircleCollider2D rangeCollider = null;
		public float Range
		{
			get => rangeCollider.radius;
			set => rangeCollider.radius = value;
		}

		[Header("Debug (Read only)")]
		[SerializeField]
		private List<EvilCell> evilCellsInRange = new List<EvilCell>();
		public List<EvilCell> EvilCellsInRange => evilCellsInRange;

		void Awake()
		{
			Range = owner.Range;
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			Debug.Log(collider.gameObject);
			EvilCell evilCell = collider.GetComponent<EvilCell>();
			if (evilCell != null)
			{
				Debug.Log("Collided with Evil body");
				evilCellsInRange.Add(evilCell);
				evilCell.AddObserver(this);

				// On cells in range
				controller.OnEnemiesInRange();
			}
		}

		void OnTriggerExit2D(Collider2D collider)
		{
			EvilCell evilCell = collider.GetComponent<EvilCell>();
			if (evilCell != null)
			{
				Debug.Log("UN-Collided with Evil body");
				evilCellsInRange.Remove(evilCell);
				evilCell.RemoveObserver(this);
				if (evilCellsInRange.Count <= 0)
				{
					controller.OnEnemiesOutOfRange();
				}
			}
		}

		public void NotifyOfDeath(EvilCell evilCell)
		{
			evilCellsInRange.Remove(evilCell);
			if (evilCellsInRange.Count <= 0)
			{
				controller.OnEnemiesOutOfRange();
			}
		}
	}
}
