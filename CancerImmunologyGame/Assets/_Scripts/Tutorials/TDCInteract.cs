using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ImmunotherapyGame.Tutorials
{
	public class TDCInteract : TutorialPopupEvent
	{

		DendriticCell cellToInteractWithPlayer = null;

		protected override void OnEndEvent()
		{
		}

		protected override void OnStartEvent()
		{
			base.OnStartEvent();
			cellToInteractWithPlayer = FindObjectOfType<DendriticCell>();
		}

		protected override bool OnUpdateEvent()
		{
			return cellToInteractWithPlayer.HasInteracted && base.OnUpdateEvent();
		}
	}
}
