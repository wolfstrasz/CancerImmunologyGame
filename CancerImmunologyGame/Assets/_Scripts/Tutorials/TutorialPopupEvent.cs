using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.Tutorials
{ 
	public class TutorialPopupEvent : TutorialEvent
	{
		[SerializeField]
		private TutorialPopup popupObject = null;

		protected override void OnEndEvent()
		{
		}

		protected override void OnStartEvent()
		{
			popupObject.Activate();
		}

		protected override bool OnUpdateEvent()
		{
			return popupObject.triggered;
		}
	}
}
