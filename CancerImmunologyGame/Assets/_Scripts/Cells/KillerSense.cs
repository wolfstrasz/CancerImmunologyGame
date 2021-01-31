using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cancers;

namespace Cells
{
	[RequireComponent(typeof(CircleCollider2D))]
	public class KillerSense : MonoBehaviour, ICancerCellObserver
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
		private List<CancerCell> cancerCellsInRange = new List<CancerCell>();
		//internal List<CancerCell> CancerCellsInRange => cancerCellsInRange;

		void Awake()
		{
			Range = owner.Range;
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			Debug.Log(collider.gameObject);
			CancerCell cc = collider.GetComponent<CancerCell>();
			if (cc != null)
			{
				Debug.Log("Collided with cc body");
				cancerCellsInRange.Add(cc);
				cc.AddObserver(this);

				// On cells in range
				controller.OnEnemiesInRange();
			}
		}

		void OnTriggerExit2D(Collider2D collider)
		{
			CancerCell cc = collider.GetComponent<CancerCell>();
			if (cc != null)
			{
				Debug.Log("UN-Collided with cc body");
				cancerCellsInRange.Remove(cc);
				cc.RemoveObserver(this);
				if (cancerCellsInRange.Count <= 0)
				{
					controller.OnEnemiesOutOfRange();
				}
			}
		}

		public void NotifyOfDeath(CancerCell cc)
		{
			cancerCellsInRange.Remove(cc);
			if (cancerCellsInRange.Count <= 0)
			{
				controller.OnEnemiesOutOfRange();
			}
		}
	}
}
