using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cells;

public class KillerCell : Cell
{
	[SerializeField]
	KillerSense sense = null;

	public void Initialise()
	{
		sense.CancerCellsInRange.Clear();
	}

	public List<CancerCell> GetCancerCellsInRange()
	{
		return sense.CancerCellsInRange;
	}

	public void ReceiveExhaustion(float value)
	{
		Debug.Log(gameObject.name + " received exhaustion of " + value);
		// Do stuff with exhaustion
	}

	public void ReceiveHealth(float value)
	{
		Debug.Log(gameObject.name + " received health of " + value);
		// Do stuff with exhaustion
	}
}
