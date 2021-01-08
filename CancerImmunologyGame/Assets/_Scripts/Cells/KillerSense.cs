using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Collider2D))]
public class KillerSense : MonoBehaviour
{
	[SerializeField]
	private KillerCell owner = null;
	[SerializeField]
	private List<CancerCell> cancerCellsInRange = new List<CancerCell>();
	public List<CancerCell> CancerCellsInRange => cancerCellsInRange;


	// Attack functionality
	private void OnTriggerEnter2D(Collider2D collider)
	{
		Debug.Log("Colliding with  " + collider.gameObject);
		CancerCellBody cell = collider.gameObject.GetComponent<CancerCellBody>();
		Debug.Log(collider.gameObject.name);
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
