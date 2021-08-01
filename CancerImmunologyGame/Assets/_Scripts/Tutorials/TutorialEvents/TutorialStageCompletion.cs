using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.Tutorials
{
	public class TutorialStageCompletion : TutorialEvent
	{
		[SerializeField] private List<TutorialStage> stageRequirements = new List<TutorialStage>();

		protected override bool OnUpdateEvent()
		{
			if (shouldPauseGameplay)
			{
				Debug.LogError("Event is waiting for a queue of events but is holding gamplay at pause! (Event: " + gameObject.name + ")");
			}

			for (int i = 0; i < stageRequirements.Count; ++i)
			{
				if (!stageRequirements[i].IsFinished)
					return false;
			}
			return true;
		}
	}
}
