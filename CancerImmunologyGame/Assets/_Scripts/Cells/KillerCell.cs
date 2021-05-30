using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ImmunotherapyGame.Core;

namespace ImmunotherapyGame
{

	public class KillerCell : Cell
	{
		[SerializeField]
		private Rigidbody2D rb = null;

		[Header("Attributes")]
		[SerializeField]
		public float movementSpeed = 4.0f;
		[SerializeField]
		private float exhaustEffectReduction = 0.75f;
		[SerializeField]
		private float immunotherapySpeedMultiplier = 1.66f;
		[SerializeField]
		private float immunotherapyEnergyRegain = 3.33f;

		[Header("Normal Attack")]
		[SerializeField]
		private GameObject normalAttackParticlePrefab = null;
		[SerializeField]
		private Transform normalAttackSpawnTransform = null;
		[SerializeField]
		private float normalAttackRange = 1.5f;
		[SerializeField]
		private float normalAttackSpreadAngle = 90.0f;
		[SerializeField]
		private float normalAttackEnergyCost = -7.5f;
		[SerializeField]
		private float normalAttackCooldown = 0.2f;
		[ReadOnly]
		private float normalAttackDowntime = 0.0f;

		[Header("Special Attack Attributes")]
		[SerializeField]
		private GameObject specialAttackParticlePrefab = null;
		[SerializeField]
		private Transform specialAttackSpawnTransform = null;
		[SerializeField]
		private float specialAttackRange = 3;
		[SerializeField]
		private float specialAttackSpreadAngle = 35f;
		[SerializeField]
		private float specialAttackEnergyCost = 0f;
		[SerializeField]
		private float specialAttackCooldown = 10f;
		[ReadOnly]
		private float specialAttackDowntime = 0;
		[SerializeField]
		private float gapDistance = 1f;
		[SerializeField]
		private int bulletAmount = 5;

		[Header("Debug only")]
		[ReadOnly]
		public ICellController controller = null;
		[ReadOnly]
		private Vector2 movementVector = Vector2.zero;
		[ReadOnly]
		private Quaternion movementRotation = Quaternion.identity;
		[ReadOnly]
		private bool isInPowerUpAnimation = false;

		// Public properties
		public float Range { get => normalAttackRange; }
		public float Fov { get => normalAttackSpreadAngle; }
		public float Health { get => health; set => health = value; }
		public float Energy { get => energy; set => energy = value; }
		public Vector2 MovementVector { get => movementVector; set => movementVector = value; }
		public Quaternion MovementRotation { get => movementRotation; set => movementRotation = value; }

		// Private proterties
		private bool CannotUseNormalAttack => NormalAttackOnCooldown || isInPowerUpAnimation;
		private bool CannotUseSpecialAttack => SpecialAttackOnCooldown || isInPowerUpAnimation;
		private bool IsMoving => !isInPowerUpAnimation && MovementVector != Vector2.zero;
		private bool NormalAttackOnCooldown => normalAttackDowntime < normalAttackCooldown;
		private bool SpecialAttackOnCooldown => specialAttackDowntime < specialAttackCooldown;

		public void OnUpdate()
		{
			float downTime = Time.deltaTime;

			if (GlobalGameData.isInPowerUpMode)
			{
				float value = immunotherapyEnergyRegain * Time.deltaTime;
				ApplyEnergyAmount(value);
			}

			if (NormalAttackOnCooldown)
			{
				normalAttackDowntime += downTime;
			}
			else
			{
				animator.SetBool("IsAttacking", false);
			}


			if (SpecialAttackOnCooldown)
			{
				specialAttackDowntime += downTime;
			}
		}

		public void OnFixedUpdate()
		{
			if (IsMoving)
				Move();
		}

		/// <summary>
		/// Applies the movement and flow vectors to calculate the new position of the cell.
		/// Note: All controllers must apply their effects before hand.
		/// </summary>
		private void Move()
		{
			movementVector *= movementSpeed * Time.fixedDeltaTime * ExhaustionEffect();

			rb.MovePosition(movementVector + rb.position);
			transform.rotation = movementRotation;
			movementVector = Vector3.zero;
			movementRotation = Quaternion.identity;
		}

		private float ExhaustionEffect()
		{
			float maxEnergy = cellType.maxEnergyValue;
			// moved from Add Energy
			animator.SetFloat("ExhaustionRate", (maxEnergy - energy) / maxEnergy);

			// before
			if (GlobalGameData.isInPowerUpMode)
				return immunotherapySpeedMultiplier;
			return 1.0f - (maxEnergy - energy) / maxEnergy * exhaustEffectReduction;
		}

		public void Attack(Vector3 targetPosition)
		{
			float spread = 0.0f;
			if (CannotUseNormalAttack) return;

			animator.SetBool("IsAttacking", true);
			normalAttackDowntime = 0.0f;
			GameObject bullet = Instantiate(normalAttackParticlePrefab, normalAttackSpawnTransform.position, Quaternion.identity);

			Vector3 bulletDirection = (targetPosition - normalAttackSpawnTransform.position).normalized;

			spread = Mathf.Lerp(Random.Range(-normalAttackSpreadAngle / 1.9f, normalAttackSpreadAngle / 1.9f), spread, Random.Range(0.2f, 0.8f));
			//spread = Random.Range(-fov /2f, fov / 2f);
			bulletDirection = Quaternion.Euler(0.0f, 0.0f, spread) * bulletDirection;

			bullet.GetComponent<KillerParticle>().Shoot(bulletDirection, normalAttackRange);

			//var color = Random.ColorHSV(0f, 1f, 0.3f, 0.6f, 0.5f, 1f);
			//if (GlobalGameData.isInPowerUpMode)
			//{
			//	bullet.GetComponent<KillerParticle>().SetMoreData(color, 2f);
			//}
			
			
			ApplyEnergyAmount(-Mathf.Abs(normalAttackEnergyCost));

		}


		public void SpecialAttack(Vector3 targetPosition)
		{
			if (CannotUseSpecialAttack) return;

			float anglePerBullet = specialAttackSpreadAngle / (bulletAmount - 1);

			float baseStartRotation = -0.5f * specialAttackSpreadAngle;

			Vector3 shootDirection = (targetPosition - transform.position);

			for (int i = 0; i < bulletAmount; ++i)
			{
				Debug.Log("Particle: " + i);
				// Find base angle
				float baseRotation = baseStartRotation + i * anglePerBullet;

				// Find angle offset
				float angleOffset = Random.Range(0f, anglePerBullet);

				float angleToRotate = baseRotation - 0.5f * anglePerBullet + angleOffset;
				// Find direction vector
				Vector3 direction = Quaternion.Euler(0.0f, 0.0f, angleToRotate) * shootDirection;

				// Find position to instantiate
				PacParticle particle = Instantiate(specialAttackParticlePrefab, specialAttackSpawnTransform.position + direction * gapDistance, Quaternion.identity).GetComponent<PacParticle>();
				particle.Shoot(direction, specialAttackRange);
			}
			specialAttackDowntime = 0f;
		}

		// POWER UP IMMUNOTHERAPY
		public void EnterPowerUpMode()
		{
			isInPowerUpAnimation = true;
			animator.SetTrigger("PowerUp");
			animator.speed = immunotherapySpeedMultiplier;
		}

		public void OnFinishPowerUpAnimation()
		{
			isInPowerUpAnimation = false;
		}

		public void ExitPowerUpMode()
		{
			animator.SetTrigger("PowerUpFinished");
			animator.speed = 1.0f;
		}

		protected override void OnCellDeath()
		{
			controller.OnCellDeath();

			ApplyHealthAmount(cellType.maxHealthValue);
			ApplyEnergyAmount(cellType.maxEnergyValue);
		}

		public override bool isImmune => isInPowerUpAnimation || isDying;

	}
}