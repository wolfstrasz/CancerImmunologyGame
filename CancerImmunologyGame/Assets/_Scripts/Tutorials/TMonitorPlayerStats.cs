using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorials
{
	public class TMonitorPlayerStats : TutorialEvent
	{
		[Header("Attributes")]
		[SerializeField]
		bool monitorHealth = false;
		[SerializeField]
		float healthValue = 50.0f;
		[SerializeField]
		bool monitorExhaustion = false;
		[SerializeField]
		float exhaustionValue = 50.0f;
		[SerializeField]
		bool invertChecks = false;

		KillerCell playerKC = null;

		protected override void OnEndEvent()
		{
			playerKC.Exhaustion = exhaustionValue;
			playerKC.Health = healthValue;
		}

		protected override void OnStartEvent()
		{
			playerKC = GlobalGameData.player.GetComponent<KillerCell>();
		}

		protected override bool OnUpdate()
		{
			if (monitorHealth && playerKC.Health <= healthValue)
				return true ^ invertChecks;
			if (monitorExhaustion && playerKC.Exhaustion >= exhaustionValue)
				return true ^ invertChecks;
			

			return false ^ invertChecks;
		}

	}


}
