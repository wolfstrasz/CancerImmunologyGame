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
	[SerializeField]
	private Animator animator = null;

	[Header("Attributes")]
	[SerializeField]
	private float speed = 4.0f;
	[SerializeField]
	private float immunotherapySpeedMultiplier = 1.66f;
	[SerializeField]
	private static float maxHealth = 100.0f;
	private float health = 100.0f;
	[SerializeField]
	private static float maxExhaustion = 100.0f;
	private float exhaustion = 0.0f;

#if BLOODFLOW_ROTATION
	[SerializeField]
	private float rotationSpeed = 2.0f;
#else
#endif

	[Header("Attack")]
	[SerializeField]
	private float attackRotationOffset = 0.2f;
	[SerializeField]
	private GameObject attackEffect = null;

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
	private List<CancerCell> cancerCellsInRange = new List<CancerCell>();
	[SerializeField]
	private CancerCell closestCell = null;

#if BLOODFLOW_ROTATION
	[SerializeField]
	private Quaternion correctRotation = Quaternion.identity;
#else
#endif

	public float Health { get => health; set => health = value; }
	public float Exhaustion { get => exhaustion; set => exhaustion = value; }
	public bool IsBusy { get => isBusy; set => isBusy = value; }
	public static float MaxExhaustion { get => maxExhaustion; set => maxExhaustion = value; }
	public static float MaxHealth { get => maxHealth; set => maxHealth = value; }
	public Vector2 MovementVector { get => movementVector; set => movementVector = value; }
	public Vector2 FlowVector { get => flowVector; set => flowVector = value; }
	public bool IsDead { get => isDead; set => isDead = value; }

#if BLOODFLOW_ROTATION
	public Quaternion CorrectRotation { get => correctRotation; set => correctRotation = value; }
#else
#endif


	public void Initialise()
	{
		sense.CancerCellsInRange.Clear();
	}

	public void OnFixedUpdate()
	{
		if (!isDead)
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
			float value = -3.33f * Time.deltaTime;
			ReceiveExhaustion(value);
		}
	}


	public void Respawn()
	{
		isDead = false;
		ReceiveHealth(maxHealth);
		ReceiveExhaustion(-maxExhaustion);
	}


	public void ReceiveExhaustion(float value)
	{
		Debug.Log(gameObject.name + " received exhaustion of " + value);
		if (GlobalGameData.isInPowerUpMode && value >= 0.0f) return;

		exhaustion += value;

		if (exhaustion >= maxExhaustion)
		{
			exhaustion = maxExhaustion;
			animator.SetFloat("ExhaustionRate", 1.0f);
			isDead = true;
			return;
		}

		if (exhaustion < 0.0f)
		{
			exhaustion = 0.0f;
			animator.SetFloat("ExhaustionRate", 0.0f);
			return;
		}

		animator.SetFloat("ExhaustionRate", exhaustion / maxExhaustion);
	}

	public void ReceiveHealth(float value)
	{
		Debug.Log(gameObject.name + " received health of " + value);

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
		return (MaxExhaustion - exhaustion) / MaxExhaustion;
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

	// ATTACKING
	//////////////////////////////////////////
	public void Attack()
	{
		if (isBusy) return;

		cancerCellsInRange = sense.CancerCellsInRange;
		if (cancerCellsInRange.Count == 0) return;

		isBusy = true;

		// Find closest cancer cell
		// Need to change to Cancer optimisation
		float minDist = 100000.0f;
		closestCell = null;

		foreach (var cell in cancerCellsInRange)
		{
			if (cell.CellInDivision()) continue;

			float dist = Vector3.Distance(transform.position, cell.transform.position);
			if (dist < minDist)
			{
				minDist = dist;
				closestCell = cell;
			}
		}

		if (closestCell == null)
		{
			isBusy = false;
			return;
		}

		animator.SetTrigger("Attacks");
	}

	public void OnAttackEffect()
	{
		Vector3 diff = closestCell.transform.position - transform.position;
		diff.Normalize();

		float rot_z = ((Mathf.Atan2(diff.y, diff.x) + attackRotationOffset) * Mathf.Rad2Deg);

		GameObject newEffect = Instantiate(attackEffect, transform.position, Quaternion.Euler(0f, 0f, rot_z));
		newEffect.GetComponent<ParticleSystem>().Play();

		if (!GlobalGameData.isInPowerUpMode)
			exhaustion += 7.5f;

		bool killedTheCell = closestCell.HitCell();
		if (killedTheCell)
		{
			cancerCellsInRange.Remove(closestCell);
		}
	}

	public void OnAttackFinished()
	{
		isBusy = false;
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
}
