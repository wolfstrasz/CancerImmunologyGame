using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.Player;

namespace ImmunotherapyGame.Tutorials
{
	public class TMonitorPlayerStats : TutorialEvent
	{
		[Header("Attributes")]
		[SerializeField]
		private bool monitorHealth = false;
		[SerializeField]
		private float healthValue = 50.0f;
		[SerializeField]
		private bool monitorEnergy = false;
		[SerializeField]
		private float energyValue = 50.0f;
		[SerializeField]
		private bool invertChecks = false;

		[Header("Debug (Read Only)")]
		[SerializeField]
		private KillerCell playerKC = null;

		protected override void OnEndEvent()
		{

		}

		protected override void OnStartEvent()
		{
			playerKC = PlayerController.Instance.ControlledCell;
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
