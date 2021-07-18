using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ImmunotherapyGame.Core;


namespace ImmunotherapyGame.Player
{
	public class PlayerUI : Singleton<PlayerUI>
	{
		[SerializeField] private PlayerData playerData = null;
		[SerializeField] private ResourceBar healthSlider = null;
		[SerializeField] private ResourceBar energySlider = null;
		[Header("Debug")]
		[SerializeField] [ReadOnly] private Cell observedCell = null;

		public void Activate()
		{
			gameObject.SetActive(true);
		}

		public void Deactivate()
		{
			gameObject.SetActive(false);
		}

		private void OnEnable()
		{
			if (playerData != null)
			{
				playerData.onCurrentCellChanged += OnCurrentObservedCellChanged;
				observedCell = playerData.CurrentCell;
			}
			else
			{
				Debug.LogWarning("Player UI is missing Player Data object.");
			}
		}

		private void OnDisable()
		{
			if (playerData != null)
			{
				playerData.onCurrentCellChanged -= OnCurrentObservedCellChanged;
				observedCell = null;
			}
			else
			{
				Debug.LogWarning("Player UI is missing Player Data object.");
			}
		}


		// TODO: Optimize with use of events to detect max health value changes
		private void Update()
		{
			if (observedCell)
			{
				healthSlider.SetValue(observedCell.CurrentHealth);
				energySlider.SetValue(observedCell.CurrentEnergy);

				if (observedCell.cellType)
				{
					// TODO: Bind to stat attributes changes
					healthSlider.SetMaxValue(observedCell.cellType.MaxHealth);
					energySlider.SetMaxValue(observedCell.cellType.MaxEnergy);
				}
			}
		}

		private void OnCurrentObservedCellChanged()
		{
			observedCell = playerData.CurrentCell;
		}


	}

}
