using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.Tutorials
{
    public class TutorialDisplayImage : TutorialEvent
    {
		[SerializeField] private TutorialLog log;
		[SerializeField] [ReadOnly] private bool isSkipped = false;

		protected override void OnEndEvent()
		{
			TutorialManager.Instance.onSkipDelegate -= Skip;
		}

		protected override void OnStartEvent()
		{
			TutorialManager.Instance.DisplayLogPanel(log);
			log.isUnlocked = true;
			TutorialManager.Instance.onSkipDelegate += Skip;
		}

		protected override bool OnUpdateEvent()
		{
			return isSkipped;
		}

		internal void Skip()
		{
			// Skips text
			isSkipped = true;
		}
	}
}
