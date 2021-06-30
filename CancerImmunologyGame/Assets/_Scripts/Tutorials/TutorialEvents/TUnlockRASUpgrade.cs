using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ImmunotherapyGame.ResearchAdvancement;

namespace ImmunotherapyGame.Tutorials
{
	public class TUnlockRASUpgrade : TutorialEvent
	{
		[SerializeField] private List<StatUpgrade> statUpgradesToUnlock = new List<StatUpgrade>();

		protected override void OnEndEvent()
		{
		}

		protected override void OnStartEvent()
		{
			ResearchAdvancementSystem.Instance.UnlockUpgrades(statUpgradesToUnlock);
		}

		protected override bool OnUpdateEvent()
		{
			return true;
		}
	}
}
