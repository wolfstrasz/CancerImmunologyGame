using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame
{
	public class RegulatorySense : MonoBehaviour
	{
		public RegulatoryCell rc = null;

		private List<KillerCell> cells = new List<KillerCell>();

		private void OnTriggerEnter2D(Collider2D collider)
		{
			KillerCell pc = collider.gameObject.GetComponent<KillerCell>();
			if (pc != null)
			{
				if (!cells.Contains(pc))
				{
					cells.Add(pc);
					rc.StartShooting();
				}

			}
		}

		private void OnTriggerExit2D(Collider2D collider)
		{
			KillerCell pc = collider.gameObject.GetComponent<KillerCell>();
			if (pc != null)
			{
				if (cells.Contains(pc))
				{
					cells.Remove(pc);

					if (cells.Count == 0)
						rc.StopShooting();

				}
			}
		}
	}
}