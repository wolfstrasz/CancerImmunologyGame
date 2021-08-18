using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using ImmunotherapyGame.Core;

namespace ImmunotherapyGame.Abilities
{
    [CreateAssetMenu(menuName = "MyAssets/Abilities/Status Effect Ability")]
    public class StatusEffectAbility : Ability
    {
		[Expandable] public StatAttribute lifetime;
		[SerializeField] private bool isStatusEffectOverTime;

		// Attributes
		public float Lifetime => (lifetime != null ? lifetime.CurrentValue : 0f);
		public bool IsStatusEffectOverTime => isStatusEffectOverTime;

		public override bool CastAbility(GameObject abilityCaster, GameObject target)
		{
			AbilityEffect abilityEffect = AbilityEffectManager.Instance.GetEffect(this);
			abilityEffect.transform.position = target.transform.position;
			abilityEffect.transform.rotation = target.transform.rotation;

			StatusEffect statusEffect = abilityEffect.GetComponent<StatusEffect>();
			statusEffect.Apply(this, target);
		
			return true;
		}

    }
}
