using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ImmunotherapyGame.Abilities;
namespace ImmunotherapyGame
{

	public class KillerCell : Cell
	{
		[Header ("Linking")]
		[SerializeField] private Rigidbody2D rb = null;
		[SerializeField] private Transform spriteTransform = null;
		[Header("Exhaustion")]
		[SerializeField] private float exhaustEffectReduction = 0.75f;

		[Header("Attacks")]
		[SerializeField] private RangedAbilityCaster primaryAbilityCaster = null;
		[SerializeField] private RangedAbilityCaster secondaryAbilityCaster = null;

		[Header("Debug")]
		[SerializeField][ReadOnly] private Vector2 movementVector = Vector2.zero;
		[SerializeField][ReadOnly] private Quaternion movementRotation = Quaternion.identity;
		[SerializeField][ReadOnly] private GameObject primaryAttackTarget = null;
		
		/* Public properties */
		public override bool isImmune => isDying;
		public Vector2 MovementVector { get => movementVector; set => movementVector = value; }
		public Quaternion MovementRotation { get => movementRotation; set => movementRotation = value; }

		public bool CanUsePrimaryAttack => primaryAbilityCaster.CanCastAbility(CurrentEnergy);
		public bool CanUseSecondaryAttack => secondaryAbilityCaster.CanCastAbility(CurrentEnergy);

		public RangedAbilityCaster PrimaryAbilityCaster => primaryAbilityCaster;
		public RangedAbilityCaster SecondaryAbilityCaster => secondaryAbilityCaster;

		public bool FlipSpriteLocalTransform 
		{ 
			set
			{
				if (value)
				{
					spriteTransform.localRotation = new Quaternion(0f, -180f, 0f, 0f);
				} 
				else
				{
					spriteTransform.localRotation = Quaternion.identity;
				}
			}

		}

		public void OnFixedUpdate()
		{
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

		/// <summary>
		/// Calculatates and returns the effect variable of low energy exhaustion.
		/// </summary>
		/// <returns></returns>
		private float ExhaustionEffect()
		{
			float maxEnergy = cellType.MaxEnergy;
			// moved from Add Energy
			animator.SetFloat("ExhaustionRate", (maxEnergy - CurrentEnergy) / maxEnergy);

			return 1.0f - (maxEnergy - CurrentEnergy) / maxEnergy * exhaustEffectReduction;
		}

		/* PRIMARY ATTACK EXECUTION METHODS */
		/// <summary>
		/// Starts the primary attack execution of the Killer Cell given a target object.
		/// </summary>
		/// <param name="target"></param>
		public void UsePrimaryAttack(GameObject target)
		{
			if (!CanUsePrimaryAttack) return;
			animator.SetBool("IsAttacking", true);
			primaryAttackTarget = target;
		}

		/// <summary>
		/// Callback for the attack animation to trigger the primary attack
		/// </summary>
		public void OnExecutePrimaryAttack()
		{
			if (!CanUsePrimaryAttack || primaryAttackTarget == null)
			{
				StopPrimaryAttack();
				return;
			}
			Debug.Log("Killer cell executes primary attack!");

			float energyCost = primaryAbilityCaster.CastAbility(primaryAttackTarget);
			ApplyEnergyAmount(-energyCost);
		}

		/// <summary>
		/// Forces the cell to leave its primary attack animation state and break the primary attack execution.
		/// </summary>
		public void StopPrimaryAttack()
		{
			animator.SetBool("IsAttacking", false);
		}

		/// <summary>
		/// Starts the secondary attack execution of the Killer Cell given a target object.
		/// </summary>
		/// <param name="target"></param>
		public void UseSecondaryAttack(GameObject target)
		{
			if (!CanUseSecondaryAttack) return;
			float energyCost = secondaryAbilityCaster.CastAbility(target);
			ApplyEnergyAmount(-energyCost);
		}

		protected override void OnCellDeath()
		{
			isDying = true;

			if (onDeathEvent != null)
			{
				onDeathEvent(this);
			}
		}

	}
}