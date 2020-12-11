using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		public GameObject PlayerPowerUpPanelActive { set => playerInfoPanel.SetActive(value); }

		[Header("Attribute bar links")]
		[SerializeField]
		private HealthBar healthBar = null;
		[SerializeField]
		private ExhaustionBar exhaustionBar = null;
		[SerializeField]
		private ImmunotherapyBar powerUpBar = null;


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




	}


}
