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

		private float health = 100;
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
		private SpriteRenderer immunotherapyIconRenderer = null;
		[SerializeField]
		private Color iconEnabledColor = Color.blue;
		[SerializeField]
		private Color iconDisabledColor = Color.gray;

		void Start()
		{
			health = maxHealth;
			if (healthBar != null)
			{
				healthBar.SetMaxValue(maxHealth);
				healthBar.SetValue(maxHealth);
			}
			else Debug.LogWarning("Health bar is not linked to global data");

			exhaustion = 0.0f;
			if (exhaustionBar != null)
			{
				exhaustionBar.SetMaxValue(maxExhaustion);
			}
			else Debug.LogWarning("Exhaust bar is not linked to global data");

			powerUp = 100.0f;
			if (powerUpBar != null)
			{
				powerUpBar.SetMaxValue(maxPowerUp);
			}
			else Debug.LogWarning("Power up bar is not linked to global data");
		}

		void Update()
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
			if (GlobalGameData.isPaused) return;

			if (!PlayerController.Instance.isInPowerUpMode)
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
			if (PlayerController.Instance.isInPowerUpMode && value >= 0.0f) return;

			exhaustion += value;

			if (exhaustion > maxExhaustion)
			{
				exhaustionBar.SetValue(exhaustion = maxExhaustion);
				PlayerController.Instance.Respawn();
				return;
			}
			
			if (exhaustion < 0.0f)
			{
				exhaustionBar.SetValue(exhaustion = 0.0f);
				return;
			}

			exhaustionBar.SetValue(exhaustion);
		}

		public void AddPowerUp(float value)
		{
			powerUp += value;

			if (powerUp > maxPowerUp)
			{
				powerUp = maxPowerUp;
				powerUpBar.SetValue(powerUp = maxPowerUp);
				immunotherapyIconRenderer.color = iconEnabledColor;
				immunotherapyButton.interactable = true;
				return;
			}
			
			if (powerUp < 0.0f)
			{
				powerUpBar.SetValue(powerUp = 0.0f);
				immunotherapyIconRenderer.color = iconDisabledColor;
				immunotherapyButton.interactable = false;
				return;
			}

			powerUpBar.SetValue(powerUp);
			immunotherapyIconRenderer.color = iconDisabledColor;
		}

		internal void ResetData()
		{
			healthBar.SetValue(health = maxHealth);
			exhaustionBar.SetValue(exhaustion = 0.0f);
			powerUpBar.SetValue(powerUp = 0.0f);
		}


		internal float GetExhaustRatio()
		{
			return (maxExhaustion - exhaustion) / maxExhaustion;
		}

		public void TriggerPowerUp()
		{
			AddPowerUp(-0.01f);
			PlayerController.Instance.EnterPowerUpMode();
		}

	}


}
