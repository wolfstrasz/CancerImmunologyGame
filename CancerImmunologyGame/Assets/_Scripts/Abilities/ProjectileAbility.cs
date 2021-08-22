using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.Core;

namespace ImmunotherapyGame.Abilities
{
	/// <summary>
	/// Ability that generates a number of projectiles with a given spread and lifetime.
	/// </summary>
	[CreateAssetMenu(menuName = "MyAssets/Abilities/Particle Ability")]
	public class ProjectileAbility : Ability
	{

		[Header("Projectile Creation")]
		[Expandable] public StatAttribute projectileCount;
		[Expandable] public StatAttribute projectileSpread;
		[Expandable] public StatAttribute projectileLifetime;
		[SerializeField] private bool useRandomSpreadOffset;

		[SerializeField] [ReadOnly] private float spreadPerParticle;
		[SerializeField] [ReadOnly] private float baseStartRotation;

		public float ProjectileCount => (projectileCount != null ? projectileCount.CurrentValue : 0f);
		public float ProjectileSpread => (projectileSpread != null ? projectileSpread.CurrentValue : 0f);
		public float ProjectileLifetime => (projectileLifetime != null ? projectileLifetime.CurrentValue : 0f);


		protected void OnEnable()
		{
			CalculateParticleSpawnValues();
			if (projectileCount != null)
				projectileCount.onValueChanged += CalculateParticleSpawnValues;
			if (projectileSpread != null)
				projectileSpread.onValueChanged += CalculateParticleSpawnValues;
		}

		protected void OnDisable()
		{
			if (projectileCount != null)
				projectileCount.onValueChanged -= CalculateParticleSpawnValues;
			if (projectileSpread != null)
				projectileSpread.onValueChanged -= CalculateParticleSpawnValues;
		}

		protected void CalculateParticleSpawnValues()
		{
			if (projectileCount == null)
				return;

			if (projectileCount.CurrentValue > 1)
			{
				spreadPerParticle = projectileSpread.CurrentValue / (projectileCount.CurrentValue - 1);
			}
			else
			{
				spreadPerParticle = projectileSpread.CurrentValue;
			}


			// Starting from -0.5f to +0.5f to represent the spread of all particles
			// so that the centre remains the attack direction
			baseStartRotation = -0.5f * projectileSpread.CurrentValue;
		}

		/// <summary>
		/// Cast the ability by generating projectiles at the ability casters position
		/// that are shot at the direction of the target.
		/// </summary>
		/// <param name="abilityCaster"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public override bool CastAbility(GameObject abilityCaster, GameObject target)
		{
			//Debug.Log("Projectile ability casted: " + name);
			Vector3 originPosition = abilityCaster.transform.position;
	
			Vector3 direction = (target.transform.position - originPosition).normalized;
			SpawnProjectiles(originPosition, direction);

			return true;
		}

		/// <summary>
		/// Generates all projectile particles at the spawn position and shoots them
		/// in a cone dependent on the ability's spread value and the given direction.
		/// </summary>
		/// <param name="spawnPosition"></param>
		/// <param name="direction"></param>
		protected void SpawnProjectiles(Vector3 spawnPosition, Vector3 direction)
		{
			float angleToRotateCurrentProjectileDirection = baseStartRotation;

			for (int i = 0; i < projectileCount.CurrentValue; ++i)
			{
				//Debug.Log("Projectile ability ( " + name + ") particle: " + (i + 1));
				// Find angle offset
				float angleOffset = 0f;
				if (useRandomSpreadOffset)
					angleOffset = Mathf.Lerp(Random.Range(spreadPerParticle * -0.5f, spreadPerParticle * 0.5f), spreadPerParticle, Random.Range(0f, 1f));

				float angleToRotateProjectileDirection = angleToRotateCurrentProjectileDirection + angleOffset;

				// Find direction vector
				Vector3 bulletDirection = Quaternion.Euler(0.0f, 0.0f, angleToRotateProjectileDirection) * direction;

				// Find position to instantiate
				AbilityEffect abilityEffect = AbilityEffectManager.Instance.GetEffect(this);
				Projectile projectile = abilityEffect.GetComponent<Projectile>();
				projectile.transform.position = spawnPosition;
				projectile.transform.rotation = Quaternion.identity;
				projectile.Shoot(bulletDirection, this);

				angleToRotateCurrentProjectileDirection += spreadPerParticle;
			}
		}
	}
}
