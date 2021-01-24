using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
	public class PlayerUI : Singleton<PlayerUI>
	{
		[Header("PowerUpStats")]
		[SerializeField]
		private float maxPowerUp = 100.0f;
		[SerializeField]
		private float powerUp = 100.0f;
		[Header("UI links")]
		[SerializeField]
		private GameObject playerInfoPanel = null;
		public GameObject PlayerInfoPanelActive { set => playerInfoPanel.SetActive(value); }

		[SerializeField]
		private GameObject playerPowerUpPanel = null;
		public GameObject PlayerPowerUpPanelActive { set => playerPowerUpPanel.SetActive(value); }

		[Header("Attribute UI links")]
		[SerializeField]
		private HealthBar healthBar = null;
		[SerializeField]
		private ExhaustionBar exhaustionBar = null;
		[SerializeField]
		private ImmunotherapyBar powerUpBar = null;
		[SerializeField]
		private GameObject microscope = null;
		[SerializeField]
		private Animator microscopeIconAnimator = null;

		[Header("Power Up Action Button")]
		[SerializeField]
		private Animator immunotherapyAnimator = null;

		[Header("Debug only")]
		[SerializeField]
		internal KillerCell kc = null;

		internal void Initialise()
		{
			if (healthBar != null)
			{
				healthBar.SetMaxValue(KillerCell.MaxHealth);

			}
			else Debug.LogWarning("Health bar is not linked to global data");

			if (exhaustionBar != null)
			{
				exhaustionBar.SetMaxValue(KillerCell.MaxExhaustion);
				//playerAnimator.SetFloat("ExhaustionRate", GetExhaustionRatio());
			}
			else Debug.LogWarning("Exhaust bar is not linked to global data");

			if (powerUpBar != null)
			{
				powerUpBar.SetMaxValue(maxPowerUp);
				powerUpBar.SetValue(powerUp = (maxPowerUp - 1.0f));
			}
			else Debug.LogWarning("Power up bar is not linked to global data");
		}


		internal void OnUpdate()
		{
			UpdateCellBars();
			IncreasePowerUpBar();

			if (powerUp == 0.0f)
			{
				GlobalGameData.isInPowerUpMode = false;

				var killerCells = FindObjectsOfType<KillerCell>();
				for (int i = 0; i < killerCells.Length; ++i)
				{
					killerCells[i].ExitPowerUpMode();
				}

			}

#if !REMOVE_PLAYER_DEBUG
			if(Input.GetKeyDown(KeyCode.R))
			{
				ActivateImmunotherapyPanel();
			}
			if (Input.GetKeyDown(KeyCode.Z))
			{
				CellpediaUI.Cellpedia.Instance.UnlockCellDescription(CellpediaCells.DENDRITIC);
			}
			if (Input.GetKeyDown(KeyCode.X))
			{
				CellpediaUI.Cellpedia.Instance.UnlockCellDescription(CellpediaCells.REGULATORY);
			}
			if (Input.GetKeyDown(KeyCode.C))
			{
				CellpediaUI.Cellpedia.Instance.UnlockCellDescription(CellpediaCells.CANCER);
			}
			if (Input.GetKeyDown(KeyCode.V))
			{
				CellpediaUI.Cellpedia.Instance.UnlockCellDescription(CellpediaCells.THELPER);
			}
#endif
		}

		private void UpdateCellBars()
		{
			healthBar.SetValue(kc.Health);
			exhaustionBar.SetValue(kc.Exhaustion);
		}


		private void IncreasePowerUpBar()
		{
			if (GlobalGameData.isGameplayPaused) return;

			if (!GlobalGameData.isInPowerUpMode)
			{
				AddPowerUp(2.0f * Time.deltaTime);
				return;
			}

			Debug.Log("Reduce power-up");
			float value = -3.33f * Time.deltaTime;
			AddPowerUp(value);
		}

		public void AddPowerUp(float value)
		{
			if (powerUp == maxPowerUp) return;
			powerUp += value;

			if (powerUp >= maxPowerUp)
			{
				powerUp = maxPowerUp;
				powerUpBar.SetValue(powerUp);
				immunotherapyAnimator.SetTrigger("CanBeUsed");
				return;
			}

			if (powerUp <= 0.0f)
			{
				powerUpBar.SetValue(powerUp = 0.0f);
				//playerAnimator.SetTrigger("PowerUpFinished");
				return;
			}

			powerUpBar.SetValue(powerUp);
		}

		public void TriggerPowerUp()
		{

			Debug.Log("PowerUpClicked");

			if (powerUp < maxPowerUp) return;
			powerUp -= 1.0f;

			GlobalGameData.isInPowerUpMode = true;
			immunotherapyAnimator.SetTrigger("Activated");

			var killerCells = FindObjectsOfType<KillerCell>();
			for (int i = 0; i < killerCells.Length; ++i)
			{
				killerCells[i].EnterPowerUpMode();
			}


		}

		public void OpenCellpedia()
		{
			CellpediaUI.Cellpedia.Instance.Open();
			MicroscopeOpened();
		}

		public void MicroscopeActivate()
		{
			microscopeIconAnimator.SetTrigger("NewItem");
		}

		private void MicroscopeOpened()
		{
			microscopeIconAnimator.SetTrigger("Opened");
		}


		public void ActivateMicroscopePanel()
		{
			microscope.SetActive(true);
		}

		public void ActivatePlayerInfoPanel()
		{
			playerInfoPanel.SetActive(true);
		}

		public void ActivateImmunotherapyPanel()
		{
			playerPowerUpPanel.SetActive(true);
			if (powerUp == maxPowerUp)
				immunotherapyAnimator.SetTrigger("CanBeUsed");
		}


		public enum PlayerUIPanels { MICROSCOPE, PLAYER_INFO, IMMUNOTHERAPY}

	}


}
