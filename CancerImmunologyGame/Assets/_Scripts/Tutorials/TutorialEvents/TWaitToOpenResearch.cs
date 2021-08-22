using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.Tutorials
{
    public class TWaitToOpenResearch : TutorialEvent
    {
		[SerializeField] private bool shouldbeOpened = false;
		protected override bool OnUpdateEvent()
		{
			return shouldbeOpened == ImmunotherapyResearchSystem.ImmunotherapyResearch.Instance.IsImmunotherapyResearchOpened;
		}
	}
}
