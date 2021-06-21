using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ImmunotherapyGame.Abilities;
namespace ImmunotherapyGame
{

	public class KillerCell : Cell
	{
		public float movementSpeed = 4.0f; // Linked to AI need to remove

		[SerializeField]
		private Rigidbody2D rb = null;

		[Header("Attributes")]
		[SerializeField]
		private float exhaustEffectReduction = 0.75f;
		//[SerializeField]
		//private float immunotherapySpeedMultiplier = 1.66f;
		//[SerializeField]
		//private float immunotherapyEnergyRegain = 3.33f;

		[Header("Normal Attack")]
		[SerializeField]
		private RangedAbilityCaster primaryAbilityCaster = null;

		[Header("Secondary Attack Attributes")]
		[SerializeField]
		private RangedAbilityCaster secondaryAbilityCaster = null;

		[Header("Debug only")]
		[ReadOnly]
		private Vector2 movementVector = Vector2.zero;
		[ReadOnly]
		private Quaternion movementRotation = Quaternion.identity;
		//[ReadOnly]
		//private bool isInPowerUpAnimation = false;
		 
		// Public properties
		public float Health { get => CurrentHealth;}
		public float Energy { get => CurrentEnergy;}
		public Vector2 MovementVector { get => movementVector; set => movementVector = value; }
		public Quaternion MovementRotation { get => movementRotation; set => movementRotation = value; }
		public override bool isImmune => /*isInPowerUpAnimation || */ isDying;

		// Private proterties
		private bool CannotUseNormalAttack => !CanUsePrimaryAttack;// || isInPowerUpAnimation;
		private bool CannotUseSpecialAttack => !CanUseSecondaryAttack;// || isInPowerUpAnimation;
		private bool IsMoving => /*!isInPowerUpAnimation  && */ MovementVector != Vector2.zero;
		private bool CanUsePrimaryAttack => primaryAbilityCaster.CanCastAbility(CurrentEnergy);
		private bool CanUseSecondaryAttack => secondaryAbilityCaster.CanCastAbility(CurrentEnergy);

		public void OnUpdate()
		{
			//if (GlobalGameData.isInPowerUpMode)
			//{
			//	float value = immunotherapyEnergyRegain * Time.deltaTime;
			//	ApplyEnergyAmount(value);
			//}
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
			movementVector *= CurrentSpeed * Time.fixedDeltaTime * ExhaustionEffect();

			rb.MovePosition(movementVector + rb.position);
			transform.rotation = movementRotation;
			movementVector = Vector3.zero;
			movementRotation = Quaternion.identity;
		}

		private float ExhaustionEffect()
		{
			float maxEnergy = cellType.MaxEnergy;
			// moved from Add Energy
			animator.SetFloat("ExhaustionRate", (maxEnergy - CurrentEnergy) / maxEnergy);

			//// before
			//if (GlobalGameData.isInPowerUpMode)
			//	return immunotherapySpeedMultiplier;
			return 1.0f - (maxEnergy - CurrentEnergy) / maxEnergy * exhaustEffectReduction;
		}

		private GameObject attackTarget;

		public void UsePrimaryAttack(GameObject target)
		{
			if (CannotUseNormalAttack) return;
			Debug.Log("Killer cell tries to use primary attacK!");
			animator.SetBool("IsAttacking", true);

			attackTarget = target;
		}

		public void OnExecutePrimaryAttack()
		{
			if (CannotUseNormalAttack) return;
			Debug.Log("Killer cell executes primary attack!");

			float energyCost = primaryAbilityCaster.CastAbility(attackTarget);
			ApplyEnergyAmount(-energyCost);
		}

		public void StopPrimaryAttack()
		{
			animator.SetBool("IsAttacking", false);
		}



		public void SecondaryAttack(GameObject target)
		{
			if (CannotUseSpecialAttack) return;
			Debug.Log("Killer cell executes secondary attack!");

			float energyCost = secondaryAbilityCaster.CastAbility(attackTarget);
			ApplyEnergyAmount(-energyCost);
		}

		// POWER UP IMMUNOTHERAPY
		public void EnterPowerUpMode()
		{
			//isInPowerUpAnimation = true;
			//animator.SetTrigger("PowerUp");
			//animator.speed = immunotherapySpeedMultiplier;
		}

		//public void OnFinishPowerUpAnimation()
		//{
		//	//isInPowerUpAnimation = false;
		//}

		public void ExitPowerUpMode()
		{
			//animator.SetTrigger("PowerUpFinished");
			//animator.speed = 1.0f;
		}

		protected override void OnCellDeath()
		{
			base.OnCellDeath();
			isDying = false;
			ApplyHealthAmount(cellType.MaxHealth);
			ApplyEnergyAmount(cellType.MaxEnergy);
		}


	}
}