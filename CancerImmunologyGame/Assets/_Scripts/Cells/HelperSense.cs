using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cells
{
	[RequireComponent(typeof(Collider2D))]
	public class HelperSense : MonoBehaviour
	{
		[SerializeField]
		private HelperTCell cell = null;


		private void OnTriggerEnter2D(Collider2D collider)
		{
			KillerCell kc = collider.gameObject.GetComponent<KillerCell>();
			if (kc != null)
			{
				cell.StartHealingOnCellsNearby();
			}
		}

		private void OnTriggerExit2D(Collider2D collider)
		{
			KillerCell kc = collider.gameObject.GetComponent<KillerCell>();
			if (kc != null)
			{
				cell.TryToStopHealingOnCellLeaving();
			}
		}

		void Update()
		{
		}

	}
}
