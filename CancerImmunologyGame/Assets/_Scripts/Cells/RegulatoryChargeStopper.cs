using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame
{
    public class RegulatoryChargeStopper : MonoBehaviour
    {
        [SerializeField] private RegulatoryCell cell;
		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (cell != null)
			{
				cell.ForceChargeStop();
			}
		}
	}
}
