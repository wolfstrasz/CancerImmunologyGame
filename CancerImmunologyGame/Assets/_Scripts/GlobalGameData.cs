using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameData : SSystem<GlobalGameData>
{
	[Header("Game attributes")]
	public bool isGameplayPaused = false;


	[Header("Player attributes")]
    public float health = 100;
    public float maxHealth = 100;
    public float exhaustion = 0;
    public float maxExhaustion = 100;

    public float powerUp = 100.0f;
    public float maxPowerUp = 100.0f;

    public bool isInPowerUpMode = false;

    [SerializeField]
    private HealthBar healthBar;
    [SerializeField]
    private ExhaustionBar exhaustionBar;
    [SerializeField]
    private ImmunotherapyBar powerUpBar;

    void Awake()
    {
        health = maxHealth;
		if (healthBar != null)
		{
			healthBar.SetMaxValue(maxHealth);
			healthBar.SetValue(maxHealth);
		} else Debug.LogWarning("Health bar is not linked to global data");

		exhaustion = 0.0f;
		if (exhaustionBar != null)
		{
			exhaustionBar.SetMaxValue(maxExhaustion);
		} else Debug.LogWarning("Exhaust bar is not linked to global data");

		powerUp = 100.0f;
		if (powerUpBar != null)
		{
			powerUpBar.SetMaxValue(maxPowerUp);
		} else Debug.LogWarning("Power up bar is not linked to global data");
	}



    public void AddHealth(float value)
    {

        health += value;
        if (health > maxHealth) health = maxHealth;
        else if (health < 0.0f) health = 0.0f;
        healthBar.SetValue(health);

        //if (isInPowerUpMode) return;
        if ( value <= 0.0f)
        {
            UIManager.Instance.StartHealthTutorial();
        }
    }

    public void AddExhaustion(float value)
    {
        if (isInPowerUpMode && value >= 0.0f) return;

        exhaustion +=value;
        if (exhaustion > maxExhaustion) exhaustion = maxExhaustion;
        else if (exhaustion < 0.0f) exhaustion = 0.0f;

        if (exhaustion > maxExhaustion / 2.0f)
        {
            UIManager.Instance.StartExhaustTutorial();
        }
        exhaustionBar.SetValue(exhaustion);
    }

    public void AddPowerUp(float value)
    {
        powerUp += value;
        if (powerUp > maxPowerUp)
        {
            powerUp = maxPowerUp;
        }
        else if (powerUp < 0.0f)
        {
            powerUp = 0.0f;
            isInPowerUpMode = false;
        }

        powerUpBar.SetValue(powerUp);
    }

   
    public void SetHealth(float value)
    {
        health = value;
        healthBar.SetValue(value);
    }

    public void SetExhaustion (float value)
    {
        exhaustion = value;
        exhaustionBar.SetValue(value);
    }

    public void SetPowerUp (float value)
    {
        powerUp = value;
        powerUpBar.SetValue(value);
    }

    void Update()
    {
        if (isGameplayPaused) return;
        if (powerUp == maxPowerUp)
        {
            UIManager.Instance.ImmunotherapyIcon.color = UIManager.Instance.ImmunotherapyCanActivateColour;
            UIManager.Instance.ImmunotherapyButton.interactable = true;
        }
        else
        {
            UIManager.Instance.ImmunotherapyIcon.color = UIManager.Instance.ImmunotherapyCannotActivateColour;
            UIManager.Instance.ImmunotherapyButton.interactable = false;
        }
        if (!isInPowerUpMode)
        {
            AddPowerUp(2.0f * Time.deltaTime);
        } 
        else
        {
            float value = -3.33f * Time.deltaTime;
            AddExhaustion(value);
            AddPowerUp(value);
        }
    }

    public bool TriggerPowerUp()
    {
        if (powerUp >= maxPowerUp)
        {
            powerUp -= 0.01f;
            powerUpBar.SetValue(powerUp);

            isInPowerUpMode = true;
            return true;
        }
        return false;
    }

}
