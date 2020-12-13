using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DendriticCell : MonoBehaviour
{

	[SerializeField]
	private DCInteractArea dcArea = null;
	[SerializeField]
	private ChemokinPath chemokinePath = null;
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
		chemokinePath.ActivateChemokines();
		interacted = true;
	}
}
