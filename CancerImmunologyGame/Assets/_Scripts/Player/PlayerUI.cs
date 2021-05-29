using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ImmunotherapyGame.Core;


namespace ImmunotherapyGame.Player
{
	public class PlayerUI : Singleton<PlayerUI>
	{

		[Header("UI links")]
		[SerializeField]
		private GameObject playerInfoPanel = null;
		[SerializeField]
		private GameObject playerPowerUpPanel = null;

		[SerializeField]
		private ResourceBar healthBar = null;
		[SerializeField]
		private ResourceBar energyBar = null;
		[SerializeField]
		private ResourceBar powerUpBar = null;
		[SerializeField]
		private Animator immunotherapyAnimator = null;

		[Header("Power-up attributes")]
		[SerializeField]
		private float maxPowerUp = 100.0f;
		[SerializeField]
		private float powerUp = 0.0f;
		[SerializeField]
		private float powerUpIncreaseValue = 2.0f;
		[SerializeField]
		private float powerUpDecreaseValue = -2.0f;


		[Header("Debug only")]
		[SerializeField]
		internal KillerCell kc = null;


		/// <summary>
		/// Sets the maximum values of the player UI bar
		/// </summary>
		internal void Initialise()
		{
			kc = PlayerController.Instance.KC;
			if (healthBar != null)
			{
				healthBar.SetMaxValue(kc.maxHealth);
			}
			else Debug.LogWarning("Health bar is not linked to global data");

			if (energyBar != null)
			{
				energyBar.SetMaxValue(kc.maxEnergy);
			}
			else Debug.LogWarning("Exhaust bar is not linked to global data");

			if (powerUpBar != null)
			{
				powerUpBar.SetMaxValue(maxPowerUp);
				powerUpBar.SetValue(powerUp = 0.0f);
			}
			else Debug.LogWarning("Power up bar is not linked to global data");
		}

		/// <summary>
		/// Sets the new killer cell and its values to be monitored.
		/// </summary>
		/// <param name="kc"></param>
		internal void SetPlayerInfo(KillerCell kc)
		{
			this.kc = kc;

			if (healthBar != null)
			{
				healthBar.SetValue(kc.Health);
			}
			else Debug.LogWarning("Health bar is not linked to global data");

			if (energyBar != null)
			{
				energyBar.SetValue(kc.Energy);
			}
			else Debug.LogWarning("Exhaust bar is not linked to global data");
		}


		internal void OnUpdate()
		{
			// Update bars
			healthBar.SetValue(kc.Health);
			energyBar.SetValue(kc.Energy);

			// Update power-up


			if (GlobalGameData.isInPowerUpMode)
			{
				AddPowerUp(powerUpDecreaseValue * Time.deltaTime);
			} 
			else if (powerUp < maxPowerUp)
			{
				AddPowerUp(powerUpIncreaseValue * Time.deltaTime);
				return;
			}
		}


		public void AddPowerUp(float value)
		{
			powerUp += value;

			if (powerUp >= maxPowerUp)
			{
				powerUpBar.SetValue(powerUp = maxPowerUp);
				immunotherapyAnimator.SetTrigger("CanBeUsed");
				return;
			}

			if (powerUp <= 0.0f)
			{
				powerUpBar.SetValue(powerUp = 0.0f);
				StopPowerUp();
			}

			powerUpBar.SetValue(powerUp);
		}

		public void TriggerPowerUp()
		{
			if (powerUp < maxPowerUp) return;
			powerUpBar.SetValue(powerUp + powerUpDecreaseValue);

			GlobalGameData.isInPowerUpMode = true;
			immunotherapyAnimator.SetTrigger("Activated");

			var killerCells = FindObjectsOfType<KillerCell>();
			for (int i = 0; i < killerCells.Length; ++i)
			{
				killerCells[i].EnterPowerUpMode();
			}
		}

		private void StopPowerUp()
		{
			GlobalGameData.isInPowerUpMode = false;

			var killerCells = FindObjectsOfType<KillerCell>();
			for (int i = 0; i < killerCells.Length; ++i)
			{
				killerCells[i].ExitPowerUpMode();
			}
			return;
		}

		public void ActivatePlayerInfoPanel()
		{
			playerInfoPanel.SetActive(true);
		}

		public void ActivatePlayerInfoPanelEnergyBar() 
		{
			energyBar.gameObject.SetActive(true);
		}

		public void ActivateImmunotherapyPanel()
		{
			playerPowerUpPanel.SetActive(true);
			if (powerUp == maxPowerUp)
			{
				immunotherapyAnimator.SetTrigger("CanBeUsed");
			}
		}
	}

	public enum PlayerUIPanels { PLAYER_INFO_ENERGYBAR, PLAYER_INFO_HEALTHBAR, IMMUNOTHERAPY, EVERYTHING }

}
