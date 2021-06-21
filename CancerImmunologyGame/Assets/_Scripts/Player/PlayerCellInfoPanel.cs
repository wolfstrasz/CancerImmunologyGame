using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ImmunotherapyGame.Player
{
    public class PlayerCellInfoPanel : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;
		[SerializeField] [ReadOnly] Cell observedCell;
		[SerializeField] private ResourceBar healthSlider = null;
		[SerializeField] private ResourceBar energySlider = null;


		private void OnEnable()
		{
			if (playerData != null)
			{

				playerData.onCurrentCellChanged += OnCurrentObservedCellChanged;
				observedCell = playerData.CurrentCell;
			}
		}

		private void OnDisable()
		{
			if (playerData != null)
			{
				playerData.onCurrentCellChanged -= OnCurrentObservedCellChanged;
				observedCell = null;

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
					healthSlider.SetMaxValue (observedCell.cellType.MaxHealth);
					energySlider.SetMaxValue (observedCell.cellType.MaxEnergy);
				}
			}
		}

		private void OnCurrentObservedCellChanged()
		{
			observedCell = playerData.CurrentCell;
		}


	}
}
