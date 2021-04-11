using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegulatorySense : MonoBehaviour
{
	public RegulatoryCell rc = null;

	private void OnTriggerEnter2D(Collider2D collider)
	{
		KillerCell pc = collider.gameObject.GetComponent<KillerCell>();
		if (pc != null)
		{
			rc.StartShooting();
		}
	}

	private void OnTriggerExit2D(Collider2D collider)
	{
		KillerCell pc = collider.gameObject.GetComponent<KillerCell>();
		if (pc != null)
		{
			rc.StopShooting();
		}
	}
}
