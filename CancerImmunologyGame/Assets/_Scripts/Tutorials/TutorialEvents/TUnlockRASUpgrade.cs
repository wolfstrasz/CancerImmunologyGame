using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ImmunotherapyGame.ImmunotherapyResearchSystem;

namespace ImmunotherapyGame.Tutorials
{
	public class TUnlockRASUpgrade : TutorialEvent
	{
		[SerializeField] private List<StatUpgrade> statUpgradesToUnlock = new List<StatUpgrade>();

		protected override void OnStartEvent()
		{
			ImmunotherapyResearch.Instance.UnlockUpgrades(statUpgradesToUnlock);
		}

	}
}
