using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cells
{
	[RequireComponent(typeof(Collider2D))]
	public class HelperSense : MonoBehaviour
	{
		[SerializeField]
		private List<KillerCell> killerCells = new List<KillerCell>();
		public List<KillerCell> KillerCells => killerCells;

		private void OnTriggerEnter2D(Collider2D collider)
		{
			KillerCell kc = collider.gameObject.GetComponent<KillerCell>();
			if (kc != null)
			{
				killerCells.Add(kc);
			}
		}

		private void OnTriggerExit2D(Collider2D collider)
		{
			KillerCell kc = collider.gameObject.GetComponent<KillerCell>();
			if (kc != null)
			{
				killerCells.Remove(kc);
			}
		}

	}
}
