using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public static class GlobalGameData
{
	public static bool isPaused = false;
	public static bool areControlsEnabled = true;
	public static bool isInitialised = false;
    public static bool isInPowerUpMode = false;
	public static float gameplaySpeed = 1.0f;
	public static float gameSpeed = 1.0f;


	public static GameObject player = null;

	public static void AddHealth(float value)
	{

		//health += value;
		//if (health > maxHealth) health = maxHealth;
		//else if (health < 0.0f) health = 0.0f;
		//healthBar.SetValue(health);
	}

	public static void AddExhaustion(float value)
	{
		//if (isInPowerUpMode && value >= 0.0f) return;

		//exhaustion += value;
		//if (exhaustion > maxExhaustion) exhaustion = maxExhaustion;
		//else if (exhaustion < 0.0f) exhaustion = 0.0f;

		//exhaustionBar.SetValue(exhaustion);
	}

	public static void AddPowerUp(float value)
	{
		//powerUp += value;
		//if (powerUp > maxPowerUp)
		//{
		//	powerUp = maxPowerUp;
		//}
		//else if (powerUp < 0.0f)
		//{
		//	powerUp = 0.0f;
		//	isInPowerUpMode = false;
		//}

		//powerUpBar.SetValue(powerUp);
	}


	public static void SetHealth(float value)
	{
		//health = value;
		//healthBar.SetValue(value);
	}

	public static void SetExhaustion(float value)
	{
		//exhaustion = value;
		//exhaustionBar.SetValue(value);
	}

	public static void SetPowerUp(float value)
	{
		//powerUp = value;
		//powerUpBar.SetValue(value);
	}

	//void Update()
 //   {

		//if (isPaused) return;

  //      if (powerUp == maxPowerUp)
  //      {
  //          UIManager.Instance.ImmunotherapyIcon.color = UIManager.Instance.ImmunotherapyCanActivateColour;
  //          UIManager.Instance.ImmunotherapyButton.interactable = true;
  //      }
  //      else
  //      {
  //          UIManager.Instance.ImmunotherapyIcon.color = UIManager.Instance.ImmunotherapyCannotActivateColour;
  //          UIManager.Instance.ImmunotherapyButton.interactable = false;
  //      }
  //      if (!isInPowerUpMode)
  //      {
  //          AddPowerUp(2.0f * Time.deltaTime);
  //      } 
  //      else
  //      {
  //          float value = -3.33f * Time.deltaTime;
  //          AddExhaustion(value);
  //          AddPowerUp(value);
  //      }
    //}

    public static bool TriggerPowerUp()
    {
		//if (powerUp >= maxPowerUp)
		//{
		//    powerUp -= 0.01f;
		//    powerUpBar.SetValue(powerUp);

		//    isInPowerUpMode = true;
		//    return true;
		//}
		//return false;
		return false;
    }

}
