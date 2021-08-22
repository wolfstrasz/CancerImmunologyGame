using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.ImmunotherapyResearchSystem;

namespace ImmunotherapyGame.Tutorials
{
    public class TDiscoverUpgrade : TutorialEvent
    {
		[SerializeField] private List<StatUpgrade> upgradesToUnlock = new List<StatUpgrade>();

		protected override void OnStartEvent()
		{
			base.OnStartEvent();

			ImmunotherapyResearch.Instance.UnlockUpgrades(upgradesToUnlock);
		}
	}
}
