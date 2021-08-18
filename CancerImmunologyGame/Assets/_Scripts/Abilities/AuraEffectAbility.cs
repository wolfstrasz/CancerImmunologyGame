using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ImmunotherapyGame.Core;
namespace ImmunotherapyGame.Abilities
{
	[CreateAssetMenu(menuName = "MyAssets/Abilities/Aura Effect Ability")]

	public class AuraEffectAbility : Ability
    {
		[Expandable] public StatAttribute lifetime;
		[Expandable] public StatAttribute auraRange;
		[SerializeField] private bool isAuraEffectOverTime;

		// Attributes
		public float Lifetime => (lifetime != null ? lifetime.CurrentValue : 0f);
		public float AuraRange => (auraRange != null ? auraRange.CurrentValue : 0f);
		public bool IsAuraEffectOverTime => isAuraEffectOverTime;

		public override bool CastAbility(GameObject abilityCaster, GameObject target )
		{
			AbilityEffect abilityEffect = AbilityEffectManager.Instance.GetEffect(this );
			AuraEffect auraEffect = abilityEffect.GetComponent<AuraEffect>();
			auraEffect.gameObject.transform.position = target.transform.position;
			auraEffect.gameObject.transform.rotation = target.transform.rotation;
			auraEffect.Apply(this, target);

			return true;
		}

	}
}
