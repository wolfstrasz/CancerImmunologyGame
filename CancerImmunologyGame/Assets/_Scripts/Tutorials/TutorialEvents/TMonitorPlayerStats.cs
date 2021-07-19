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
		private PlayerData playerData = null;
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
		private Cell playerCell = null;

		protected override void OnEndEvent()
		{
			if (playerData != null)
			{
				playerData.onCurrentCellChanged -= OnPlayerCellChanged;
			}
			else
			{
				Debug.Log("Monitoring Player Stats is missing Player Data object!");
			}
		}

		protected override void OnStartEvent()
		{
			if (playerData != null)
			{
				playerData.onCurrentCellChanged += OnPlayerCellChanged;
				if (playerData.CurrentCell != null)
				{
					playerCell = playerData.CurrentCell;
				}
			} 
			else
			{
				Debug.Log("Monitoring Player Stats is missing Player Data object!");
			}
		}

		private void OnPlayerCellChanged()
		{
			playerCell = playerData.CurrentCell;
		}

		protected override bool OnUpdateEvent()
		{
			if (playerCell == null) return false;

			if (monitorHealth && playerCell.CurrentHealth <= healthValue)
				return true ^ invertChecks;
			if (monitorEnergy && playerCell.CurrentEnergy <= energyValue)
				return true ^ invertChecks;
			

			return false ^ invertChecks;
		}

	}


}
