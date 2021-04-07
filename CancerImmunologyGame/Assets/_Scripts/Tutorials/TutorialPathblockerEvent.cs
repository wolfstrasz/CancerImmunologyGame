using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.Tutorials
{
	public class TutorialPathblockerEvent : TutorialEvent
	{
		[SerializeField]
		List<TutorialPathblocker> pathBlockers = new List<TutorialPathblocker>();
		[SerializeField]
		private PathblockingFunction function = PathblockingFunction.RELEASE;

		protected override void OnEndEvent()
		{
		}

		protected override void OnStartEvent()
		{
			if (function == PathblockingFunction.RELEASE)
			{
				foreach (TutorialPathblocker blocker in pathBlockers)
				{
					blocker.gameObject.SetActive(false);
				}
			}
			else
			{
				foreach (TutorialPathblocker blocker in pathBlockers)
				{
					blocker.gameObject.SetActive(true);
				}
			}
		}

		protected override bool OnUpdateEvent()
		{
			return true;
		}


		private enum PathblockingFunction { RELEASE, LOCK }
	}

}

