using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
	public class PlayerUI : Singleton<PlayerUI>
	{
		[Header("Player attributes")]
		[SerializeField]
		private float maxHealth = 100.0f;
		[SerializeField]
		private float maxExhaustion = 100.0f;
		[SerializeField]
		private float maxPowerUp = 100.0f;

		private float health = 100.0f;
		public float Health => health;

		private float exhaustion = 0;
		public float Exhaustion => exhaustion;

		private float powerUp = 100.0f;
		public float PowerUp => powerUp;

		[Header("UI links")]
		[SerializeField]
		private GameObject playerInfoPanel = null;
		public GameObject PlayerInfoPanelActive { set => playerInfoPanel.SetActive(value); }

		[SerializeField]
		private GameObject playerPowerUpPanel = null;
		public GameObject PlayerPowerUpPanelActive { set => playerPowerUpPanel.SetActive(value); }

		[Header("Attribute bar links")]
		[SerializeField]
		private HealthBar healthBar = null;
		[SerializeField]
		private ExhaustionBar exhaustionBar = null;
		[SerializeField]
		private ImmunotherapyBar powerUpBar = null;

		[Header("Power Up Action Button")]
		[SerializeField]
		private Button immunotherapyButton = null;
		[SerializeField]
		private Image immunotherapyIcon = null;
		[SerializeField]
		private Color iconEnabledColor = Color.blue;
		[SerializeField]
		private Color iconDisabledColor = Color.gray;

		internal Animator playerAnimator = null;

		internal void Initialise(Animator anim)
		{
			playerAnimator = anim;
			if (healthBar != null)
			{
				healthBar.SetMaxValue(maxHealth);
				healthBar.SetValue(health = maxHealth);
			}
			else Debug.LogWarning("Health bar is not linked to global data");

			if (exhaustionBar != null)
			{
				exhaustionBar.SetMaxValue(maxExhaustion);
				exhaustionBar.SetValue(exhaustion = 0.0f);
				playerAnimator.SetFloat("ExhaustionRate", GetExhaustionRatio());
			}
			else Debug.LogWarning("Exhaust bar is not linked to global data");

			if (powerUpBar != null)
			{
				powerUpBar.SetMaxValue(maxPowerUp);
				powerUpBar.SetValue(powerUp = 100.0f);
				immunotherapyIcon.color = iconEnabledColor;
			}
			else Debug.LogWarning("Power up bar is not linked to global data");
		}

		internal void OnUpdate()
		{

			IncreasePowerUpBar();
			if (Input.GetKey(KeyCode.Keypad7))
			{
				AddHealth(-0.1f);
			}
			if (Input.GetKey(KeyCode.Keypad9))
			{
				AddHealth(+0.1f);
			}
			if (Input.GetKey(KeyCode.Keypad4))
			{
				AddExhaustion(-0.1f);
			}
			if (Input.GetKey(KeyCode.Keypad6))
			{
				AddExhaustion(+0.1f);
			}
		}
		
		private void IncreasePowerUpBar()
		{
			if (GlobalGameData.isGameplayPaused) return;

			if (!GlobalGameData.isInPowerUpMode)
			{
				AddPowerUp(2.0f * Time.deltaTime);
				return;
			}

			float value = -3.33f * Time.deltaTime;
			AddExhaustion(value);
			AddPowerUp(value);
		}

		public void AddHealth(float value)
		{
			health += value;
			if (health > maxHealth)
			{
				healthBar.SetValue(health = maxHealth);
				return;
			}

			if (health < 0.0f)
			{
				healthBar.SetValue(health = 0.0f);
				PlayerController.Instance.Respawn();
			}

			healthBar.SetValue(health);
		}

		public void AddExhaustion(float value)
		{
			if (GlobalGameData.isInPowerUpMode && value >= 0.0f) return;

			exhaustion += value;

			if (exhaustion > maxExhaustion)
			{
				exhaustionBar.SetValue(exhaustion = maxExhaustion);
				playerAnimator.SetFloat("ExhaustionRate", GetExhaustionRatio());
				PlayerController.Instance.Respawn();
				return;
			}
			
			if (exhaustion < 0.0f)
			{
				exhaustionBar.SetValue(exhaustion = 0.0f);
				playerAnimator.SetFloat("ExhaustionRate", GetExhaustionRatio());
				return;
			}

			exhaustionBar.SetValue(exhaustion);
			playerAnimator.SetFloat("ExhaustionRate", GetExhaustionRatio());
		}

		public void AddPowerUp(float value)
		{
			powerUp += value;

			if (powerUp > maxPowerUp)
			{
				powerUp = maxPowerUp;
				powerUpBar.SetValue(powerUp = maxPowerUp);
				immunotherapyIcon.color = iconEnabledColor;
				return;
			}
			
			if (powerUp < 0.0f)
			{
				powerUpBar.SetValue(powerUp = 0.0f);
				playerAnimator.SetTrigger("PowerUpFinished");
				return;
			}

			powerUpBar.SetValue(powerUp);
			immunotherapyIcon.color = iconDisabledColor;
		}

		internal void ResetData()
		{
			healthBar.SetValue(health = maxHealth);
			exhaustionBar.SetValue(exhaustion = 0.0f);
			playerAnimator.SetFloat("ExhaustionRate", GetExhaustionRatio());
			powerUpBar.SetValue(powerUp = 0.0f);
		}


		internal float GetExhaustionRatio()
		{
			return exhaustion / maxExhaustion;
		}


		public void TriggerPowerUp()
		{

			Debug.Log("PowerUpClicked");

			if (powerUp < maxPowerUp) return;
			AddPowerUp(-0.01f);
			immunotherapyIcon.color = iconDisabledColor;
			PlayerController.Instance.EnterPowerUpMode();


		}

	}


}
