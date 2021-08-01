using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ImmunotherapyGame.Tutorials
{
	public class TutorialDCInteraction : TutorialEvent
	{
		[SerializeField] private DendriticCell cellToInteract = null;

		protected override bool OnUpdateEvent()
		{
			return cellToInteract.HasInteracted;
		}
	}
}
