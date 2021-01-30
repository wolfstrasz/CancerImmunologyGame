using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cancers;

namespace Cells
{
	[RequireComponent(typeof(Collider2D))]
	public class KillerSense : MonoBehaviour
	{
		[Header("Debug (Read only)")]
		[SerializeField]
		private List<CancerCell> cancerCellsInRange = new List<CancerCell>();
		internal List<CancerCell> CancerCellsInRange => cancerCellsInRange;


		// Attack functionality
		private void OnTriggerEnter2D(Collider2D collider)
		{
			CancerCellBody cell = collider.gameObject.GetComponent<CancerCellBody>();
			if (cell != null)
			{
				cancerCellsInRange.Add(cell.owner);
			}
		}

		private void OnTriggerExit2D(Collider2D collider)
		{
			CancerCellBody cell = collider.gameObject.GetComponent<CancerCellBody>();
			if (cell != null)
			{
				cancerCellsInRange.Remove(cell.owner);
			}
		}

	}
}
