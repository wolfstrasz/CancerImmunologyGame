using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DendriticCell : MonoBehaviour
{

	[SerializeField]
	private DCInteractArea dcArea = null;
	[SerializeField]
	private List<ChemokinPath> chemokinePaths = new List<ChemokinPath>();
	[SerializeField]
	private bool interacted = false;
	public bool HasInteracted => interacted;

	public void StartInteraction()
	{
		// Start the interaction

		// TODO: remove this after adding the interaction animation/video/ect.
		FinishInteraction();
	}


	private void FinishInteraction()
	{
		foreach (ChemokinPath path in chemokinePaths)
		{
			path.ActivateChemokines();
		}
		interacted = true;
	}
}
