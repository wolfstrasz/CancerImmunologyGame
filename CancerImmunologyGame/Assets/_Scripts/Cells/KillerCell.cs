using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cells;

public class KillerCell : Cell
{
	[SerializeField]
	private Rigidbody2D rb = null;
	[SerializeField]
	private KillerSense sense = null;

	[Header("Attributes")]
	[SerializeField]
	private float exhaustEffectReduction = 0.75f;
	[SerializeField]
	public float movementSpeed = 4.0f;
	[SerializeField]
	private float immunotherapySpeedMultiplier = 1.66f;
	[SerializeField]
	private float immunotherapyEnergyRegain = 3.33f;

	[Header("Normal Attack")]
	[SerializeField]
	private GameObject attackSpawnObject = null;
	[SerializeField]
	private GameObject killerParticlePrefab = null;
	[SerializeField]
	private float normalAttackEnergyCost = -7.5f;
	[SerializeField]
	private float attackCooldown = 0.2f;
	private float attackDowntime = 0.0f;
	private bool canAttack = true;

	[SerializeField]
	private float range = 1.5f;
	[SerializeField]
	private float fov = 90.0f;

	[Header("Debug only")]
	[SerializeField]
	private bool isBusy = false;
	[SerializeField]
	private bool queuePowerUp = false;
	[SerializeField]
	private Vector2 movementVector = Vector2.zero;
	[SerializeField]
	private Quaternion movementRotation = Quaternion.identity;

	[SerializeField]
	private GameObject spriteObject = null;

	[SerializeField]
	public ICellController controller = null;

	public Quaternion SpriteOrientation 
	{
		get => spriteObject.transform.rotation;
		set 
		{
			//Debug.Log(value.eulerAngles);
			if (value == Quaternion.identity)
			{
				spriteObject.transform.localRotation = value;
				spriteObject.transform.localScale = Vector3.one;
			}
			else if (value.eulerAngles.z >= 90.0f && value.eulerAngles.z <= 270.0f)
			{
				spriteObject.transform.localRotation = Quaternion.Euler(0.0f, 0.0f , value.eulerAngles.z);
				spriteObject.transform.localScale = new Vector3(-1.0f, -1.0f, 1.0f);
			} else
			{
				spriteObject.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, value.eulerAngles.z);
				spriteObject.transform.localScale = new Vector3 (-1.0f , 1.0f , 1.0f);

			}
		} 
	}

	[SerializeField]
	private Quaternion correctRotation = Quaternion.identity;

	public KillerSense Sense { get => sense; }
	public float Range { get => range; }
	public float Fov { get => fov; }
	public float Health { get => health; set => health = value; }
	public float Energy { get => energy; set => energy = value; }
	public Vector2 MovementVector { get => movementVector; set => movementVector = value; }
	public Quaternion MovementRotation { get => movementRotation; set => movementRotation = value; }

	public override bool isImmune => throw new System.NotImplementedException();

	public void OnFixedUpdate()
	{

		if (isBusy) return;
		Move();
	}


	public void OnUpdate()
	{


		if (GlobalGameData.isInPowerUpMode)
		{
			float value = immunotherapyEnergyRegain * Time.deltaTime;
			AddEnergy(value);

			if (!canAttack)
			{
				attackDowntime += Time.deltaTime * immunotherapySpeedMultiplier;
				if (attackDowntime >= attackCooldown)
				{
					canAttack = true;
				}
			}
			return;
		}

		if (!canAttack)
		{
			attackDowntime += Time.deltaTime;
			if (attackDowntime >= attackCooldown)
			{
				canAttack = true;
			}
		}

		if (!isBusy && canAttack)
			animator.SetBool("IsAttacking", false);
	}

	public void AddEnergy(float value)
	{
		energy += value;
		if (energy >= maxEnergy)
		{
			energy = maxEnergy;
		}

		// moved to Exhaust Effect
		// animator.SetFloat("ExhaustionRate", (maxEnergy - energy) / maxEnergy);
	}

	public void AddHealth(float value)
	{
		health += value;
		if (health > maxHealth)
		{
			health = maxHealth;
			return;
		}
	}

	private float ExhaustionEffect()
	{
		// moved from Add Energy
		animator.SetFloat("ExhaustionRate", (maxEnergy - energy) / maxEnergy);

		// before
		if (GlobalGameData.isInPowerUpMode)
			return immunotherapySpeedMultiplier;
		return  1.0f - (maxEnergy - energy) / maxEnergy * exhaustEffectReduction;
	}

	/// <summary>
	/// Applies the movement and flow vectors to calculate the new position of the cell.
	/// Note: All controllers must apply their effects before hand.
	/// </summary>
	private void Move()
	{
		movementVector = movementVector * movementSpeed * Time.fixedDeltaTime * ExhaustionEffect();

		rb.MovePosition(movementVector + rb.position);
		transform.rotation = movementRotation;
		movementVector = Vector3.zero;
		movementRotation = Quaternion.identity;
	}


	private float spread = 0.0f;
	public void Attack(Vector3 targetPosition)
	{
		if (isBusy) return;
		if (!canAttack) return;

		animator.SetBool("IsAttacking", true);
		attackDowntime = 0.0f;
		canAttack = false;
		GameObject bullet = Instantiate(killerParticlePrefab, attackSpawnObject.transform.position, Quaternion.identity);

		Vector3 bulletDirection = (targetPosition - attackSpawnObject.transform.position).normalized;

		spread =  Mathf.Lerp( Random.Range(-fov /1.9f, fov / 1.9f), spread, Random.Range(0.2f, 0.8f));
		//spread = Random.Range(-fov /2f, fov / 2f);
		bulletDirection = Quaternion.Euler(0.0f, 0.0f, spread) * bulletDirection;
		var color = Random.ColorHSV(0f, 1f, 0.3f, 0.6f, 0.5f, 1f); 
		bullet.GetComponent<KillerParticle>().Shoot(bulletDirection, range, color);
		ExhaustCell(Mathf.Abs(normalAttackEnergyCost));

	}

	public void StopAttack()
	{
		animator.SetBool("IsAttacking", false);
	}

	// POWER UP IMMUNOTHERAPY
	//////////////////////////////////////////
	public void EnterPowerUpMode()
	{
		if (isBusy)
		{
			queuePowerUp = true;
			return;
		}

		isBusy = true;
		animator.SetTrigger("PowerUp");
		animator.speed = immunotherapySpeedMultiplier;
	}

	public void OnFinishPowerUpAnimation()
	{
		isBusy = false;
		if (queuePowerUp)
			EnterPowerUpMode();
	}
	
	public void ExitPowerUpMode()
	{
		animator.SetTrigger("PowerUpFinished");
		animator.speed = 1.0f;
	}

	public override void HitCell(float amount)
	{
		//if (isImmune) return;

		health -= amount;
		if (health <= 0.0f)
		{
			health = 0.0f;
			controller.OnCellDeath();
		}
	}

	public override void ExhaustCell(float amount)
	{
		if (GlobalGameData.isInPowerUpMode) return;

		//if (isImmune) return;

		// moved to Exhaust Effect
		// animator.SetFloat("ExhaustionRate", (maxEnergy - energy) / maxEnergy);
		energy -= amount;
		if (energy <= 0.0f)
		{
			energy = 0.0f;
			controller.OnCellDeath();
		}

	}
}
