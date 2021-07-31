using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ImmunotherapyGame.Abilities;

namespace ImmunotherapyGame
{
    public class StatusEffect : MonoBehaviour
    {
        [Header("Linking")]
		[SerializeField] protected Animator animator = null;
        [SerializeField] protected SpriteRenderer render = null;

		[Header("Attributes")]
        [SerializeField] [ReadOnly] protected bool isEffectOverTime;
        [SerializeField] [ReadOnly] protected Cell ownerCell = null;
		[SerializeField] [ReadOnly] protected GameObject owner = null;
		[SerializeField] [ReadOnly] protected StatusEffectAbility ability = null;
		[SerializeField] [ReadOnly] protected float lifetime = 0f;


		protected virtual void FixedUpdate()
		{
			OnFixedUpdate();
		}

		protected virtual void OnLifeEnded()
		{

			if (!isEffectOverTime)
			{
				ability.UndoAbilityEffect(ownerCell);
			}
			Destroy(gameObject);
		}

		protected virtual void OnFixedUpdate()
		{
			if (owner == null)
			{
				Destroy(gameObject);
				return;
			}

			transform.position = owner.transform.position;

			if (isEffectOverTime)
			{
				ability.ApplyAbilityEffect(ownerCell, Time.fixedDeltaTime);
			}

			if (lifetime > 0)
			{
				lifetime -= Time.fixedDeltaTime;
				if (lifetime <= 0)
				{
					OnLifeEnded();
				}
			}
		}

		protected virtual void ApplyInitialEffect()
		{
			ability.ApplyAbilityEffect(ownerCell);
		}

		protected virtual void UndoInitialEffect()
		{
			ability.UndoAbilityEffect(ownerCell);
		}


		public virtual void Apply(StatusEffectAbility _statusEffectAbility, GameObject _owner)
		{
			owner = _owner;
			ownerCell = _owner.GetComponent<Cell>();
			ability = _statusEffectAbility;
			lifetime = ability.Lifetime;
			isEffectOverTime = ability.IsStatusEffectOverTime;

			if (ownerCell == null)
			{
				Debug.LogWarning("Status effect (" + gameObject.name + ") applied to " + _owner.name + " does not operate on a cell.");
			}

			if (!isEffectOverTime)
			{
				ability.ApplyAbilityEffect(ownerCell);
			} 
			else
			{
				if (ability.CanHitCellType(ownerCell.cellType))
				{
					ApplyInitialEffect();
				}
			}
		}
	}
}
