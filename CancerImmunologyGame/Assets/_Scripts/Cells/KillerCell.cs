using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cells;
using Cancers;

public class KillerCell : Cell
{
	[SerializeField]
	private Rigidbody2D rb = null;
	[SerializeField]
	private KillerSense sense = null;
	[SerializeField]
	private Animator animator = null;

	[Header("Attributes")]
	[SerializeField]
	private float speed = 4.0f;
	[SerializeField]
	private float immunotherapySpeedMultiplier = 1.66f;
	[SerializeField]
	private float immunotherapyEnergyRegain = 3.33f;

	[SerializeField]
	private static float maxHealth = 100.0f;
	private float health = 100.0f;
	[SerializeField]
	private static float maxEnergy = 100.0f;
	private float energy = 100.0f;

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

#if BLOODFLOW_ROTATION
	[SerializeField]
	private float rotationSpeed = 2.0f;
#else
#endif

	[Header("Debug only")]
	[SerializeField]
	private bool isBusy = false;
	[SerializeField]
	private bool isDead = false;
	[SerializeField]
	private bool queuePowerUp = false;
	[SerializeField]
	private Vector2 movementVector = Vector2.zero;
	[SerializeField]
	private Vector2 flowVector = Vector2.zero;

	[SerializeField]
	private GameObject spriteObject = null;

	[SerializeField]
	public ICellController controller = null;

	public Quaternion SpriteOrientation 
	{
		get => spriteObject.transform.rotation;
		set 
		{
			Debug.Log(value.eulerAngles);
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

#if BLOODFLOW_ROTATION
	[SerializeField]
	private Quaternion correctRotation = Quaternion.identity;
#else
#endif
	public KillerSense Sense { get => sense; }
	public float Range { get => range; }
	public float Fov { get => fov; }
	public float Health { get => health; set => health = value; }
	public float Energy { get => energy; set => energy = value; }
	public bool IsBusy { get => isBusy; set => isBusy = value; }
	public static float MaxEnergy { get => maxEnergy; set => maxEnergy = value; }
	public static float MaxHealth { get => maxHealth; set => maxHealth = value; }
	public Vector2 MovementVector { get => movementVector; set => movementVector = value; }
	public Vector2 FlowVector { get => flowVector; set => flowVector = value; }
	public bool IsDead { get => isDead; set => isDead = value; }

#if BLOODFLOW_ROTATION
	public Quaternion CorrectRotation { get => correctRotation; set => correctRotation = value; }
#else
#endif

	public void OnFixedUpdate()
	{
		if (!isDead && GlobalGameData.areControlsEnabled)
		{
			Move();
#if BLOODFLOW_ROTATION
			FixRotation();
#else
#endif

}
	}


#if BLOODFLOW_ROTATION
	private void FixRotation()
	{
		transform.rotation = Quaternion.Slerp(transform.rotation, correctRotation, rotationSpeed * Time.deltaTime);
	}
#else
#endif

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


	public void Respawn()
	{
		isDead = false;
		AddHealth(maxHealth);
		AddEnergy(maxEnergy);
	}


	public void AddEnergy(float value)
	{
		if (GlobalGameData.isInPowerUpMode && value <= 0.0f) return;

		energy += value;

		if (energy >= maxEnergy)
		{
			energy = maxEnergy;
		}
		else if (energy <= 0.0f)
		{
			energy = 0.0f;
			isDead = true;
		}

		animator.SetFloat("ExhaustionRate", (maxEnergy - energy) / maxEnergy);
	}

	public void AddHealth(float value)
	{
		health += value;
		if (health > maxHealth)
		{
			health = maxHealth;
			return;
		}

		if (health <= 0.0f)
		{
			health = 0.0f;
			isDead = true;
			
		}
	}

	private float ExhaustionEffect()
	{
		if (GlobalGameData.isInPowerUpMode)
			return immunotherapySpeedMultiplier;
		return  energy / maxEnergy;
	}

	/// <summary>
	/// Applies the movement and flow vectors to calculate the new position of the cell.
	/// Note: All controllers must apply their effects before hand.
	/// </summary>
	private void Move()
	{
		movementVector = movementVector * speed * Time.fixedDeltaTime * ExhaustionEffect();
		rb.MovePosition(movementVector + flowVector + rb.position);
	}


	private float spread = 0.0f;
	public void Attack(Vector3 targetPosition)
	{
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
		AddEnergy(normalAttackEnergyCost);

	}

	public void StopAttack()
	{
		animator.SetBool("IsAttacking", false);
	}

	//public void Attack (Vector3 direction)
	//{
	//	animator.SetBool("IsAttacking", true);
	//}


	//public void StopAttack()
	//{
	//	animator.SetBool("IsAttacking", false);
	//}
	//// ATTACKING
	//////////////////////////////////////////
	//public void Attack()
	//{
	//	if (isBusy) return;

	//	cancerCellsInRange = sense.CancerCellsInRange;
	//	if (cancerCellsInRange.Count == 0) return;

	//	isBusy = true;

	//	// Find closest cancer cell
	//	// Need to change to Cancer optimisation
	//	float minDist = 100000.0f;
	//	closestCell = null;

	//	foreach (var cell in cancerCellsInRange)
	//	{
	//		if (cell.InDivision) continue;

	//		float dist = Vector3.Distance(transform.position, cell.transform.position);
	//		if (dist < minDist)
	//		{
	//			minDist = dist;
	//			closestCell = cell;
	//		}
	//	}

	//	if (closestCell == null)
	//	{
	//		isBusy = false;
	//		return;
	//	}

	//	animator.SetTrigger("Attacks");
	//}

	//public void OnAttackEffect()
	//{
	//	Vector3 diff = closestCell.transform.position - transform.position;
	//	diff.Normalize();

	//	float rot_z = ((Mathf.Atan2(diff.y, diff.x) + attackRotationOffset) * Mathf.Rad2Deg);

	//	GameObject newEffect = Instantiate(attackEffect, transform.position, Quaternion.Euler(0f, 0f, rot_z));
	//	newEffect.GetComponent<ParticleSystem>().Play();

	//	AddEnergy(normalAttackEnergyCost);

	//	bool killedTheCell = closestCell.HitCell();
	//	if (killedTheCell)
	//	{
	//		cancerCellsInRange.Remove(closestCell);
	//	}
	//}

	//public void OnAttackFinished()
	//{
	//	isBusy = false;
	//}

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
}
