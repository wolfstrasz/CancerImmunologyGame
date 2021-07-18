using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.ImmunotherapyResearchSystem;


namespace ImmunotherapyGame.Tutorials
{
	public class TAddRASPoints : TutorialEvent
	{
		[SerializeField] private int pointsToAdd;

		protected override void OnEndEvent()
		{
		}

		protected override void OnStartEvent()
		{
			ImmunotherapyResearch.Instance.AddPoints(pointsToAdd);
		}

		protected override bool OnUpdateEvent()
		{
			return true;
		}
	}
}
