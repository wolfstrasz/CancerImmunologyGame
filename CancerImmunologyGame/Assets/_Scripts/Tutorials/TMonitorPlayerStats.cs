using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Tutorials
{
	public class TMonitorPlayerStats : TutorialEvent
	{
		[Header("Attributes")]
		[SerializeField]
		private bool monitorHealth = false;
		[SerializeField]
		private float healthValue = 50.0f;
		[SerializeField]
		private bool updateHealth = false;
		[SerializeField]
		private bool monitorEnergy = false;
		[SerializeField]
		private float energyValue = 50.0f;
		[SerializeField]
		private bool updateEnergy = false;
		[SerializeField]
		private bool invertChecks = false;

		[Header("Debug (Read Only)")]
		[SerializeField]
		private KillerCell playerKC = null;

		protected override void OnEndEvent()
		{
			if (updateEnergy)
				playerKC.Energy = energyValue;
			if (updateHealth)
				playerKC.Health = healthValue;
		}

		protected override void OnStartEvent()
		{
			playerKC = PlayerController.Instance.KC;
		}

		protected override bool OnUpdateEvent()
		{
			if (monitorHealth && playerKC.Health <= healthValue)
				return true ^ invertChecks;
			if (monitorEnergy && playerKC.Energy <= energyValue)
				return true ^ invertChecks;
			

			return false ^ invertChecks;
		}

	}


}
