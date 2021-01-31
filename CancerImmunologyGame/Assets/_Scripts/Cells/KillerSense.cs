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


		void OnTriggerEnter2D(Collider2D collider)
		{
			Debug.Log(collider.gameObject);
			CancerCellBody ccBody = collider.GetComponent<CancerCellBody>();
			if (ccBody != null)
			{
				Debug.Log("Collided with cc body");
				cancerCellsInRange.Add(ccBody.owner);
				ccBody.owner.AddObserver(this);

				// On cells in range
				controller.OnEnemiesInRange();
			}
		}

		void OnTriggerExit2D(Collider2D collider)
		{
			CancerCellBody ccBody = collider.GetComponent<CancerCellBody>();
			if (ccBody != null)
			{
				Debug.Log("UN-Collided with cc body");
				cancerCellsInRange.Remove(ccBody.owner);
				ccBody.owner.RemoveObserver(this);
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
