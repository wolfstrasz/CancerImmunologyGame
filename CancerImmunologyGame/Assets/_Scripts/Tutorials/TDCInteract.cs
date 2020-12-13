using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Tutorials
{
	public class TDCInteract : TutorialEvent
	{

		DendriticCell cellToInteractWithPlayer = null;

		protected override void OnEndEvent()
		{
		}

		protected override void OnStartEvent()
		{
			cellToInteractWithPlayer = FindObjectOfType<DendriticCell>();
		}

		protected override bool OnUpdate()
		{
			return cellToInteractWithPlayer.HasInteracted;
		}
	}
}
