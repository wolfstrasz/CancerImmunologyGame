using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ImmunotherapyGame.Core;
namespace ImmunotherapyGame.Abilities
{
	[CreateAssetMenu(menuName = "MyAssets/Abilities/Aura Effect Ability")]

	public class AuraAbility : Ability
    {
		[Expandable] public StatAttribute lifetime;
		[Expandable] public StatAttribute auraRange;
		[SerializeField] private bool isAuraEffectOverTime;

		// Attributes
		/// <summary>
		/// Returns the lifetime of the aura ability or 0 if ability does not have a lifetime.
		/// </summary>
		public float Lifetime => (lifetime != null ? lifetime.CurrentValue : 0f);
		/// <summary>
		/// Returns the range of the aura or 0 if ability has no range.
		/// </summary>
		public float AuraRange => (auraRange != null ? auraRange.CurrentValue : 0f);
		/// <summary>
		/// Returns true if the ability is set to effect cells overtime.
		/// </summary>
		public bool IsAuraEffectOverTime => isAuraEffectOverTime;

		/// <summary>
		/// Casts the ability by creating an aura ability effect on the given target.
		/// </summary>
		/// <param name="abilityCaster"></param>
		/// <param name="target"></param>
		/// <returns></returns>
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
