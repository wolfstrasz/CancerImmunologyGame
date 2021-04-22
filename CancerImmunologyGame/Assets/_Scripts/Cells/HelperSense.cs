using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame
{
	[RequireComponent(typeof(Collider2D))]
	public class HelperSense : MonoBehaviour
	{
		[SerializeField]
		private HelperTCell cell = null;

		[SerializeField]
		private List<KillerCell> kcNearby = new List<KillerCell>();

		private void OnTriggerEnter2D(Collider2D collider)
		{
			KillerCell kc = collider.gameObject.GetComponent<KillerCell>();
			if (kc != null)
			{
				cell.StartHealingOnCellsNearby();
				kcNearby.Add(kc);
			}
		}

		private void OnTriggerExit2D(Collider2D collider)
		{
			KillerCell kc = collider.gameObject.GetComponent<KillerCell>();
			if (kc != null)
			{
				kcNearby.Remove(kc);
				if (kcNearby.Count == 0)
				{
					cell.TryToStopHealingOnCellLeaving();
				}
			}
		}

		void Update()
		{
		}

	}

}