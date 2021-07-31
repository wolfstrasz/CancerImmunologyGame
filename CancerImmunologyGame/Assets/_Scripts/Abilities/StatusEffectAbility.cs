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
		[SerializeField] private GameObject statusEffectPrefab;

		// Attributes
		public float Lifetime => (lifetime != null ? lifetime.CurrentValue : 0f);
		public bool IsStatusEffectOverTime => isStatusEffectOverTime;

		public override bool CastAbility(GameObject abilityCaster, GameObject target)
		{
			StatusEffect statusEffect = Instantiate(statusEffectPrefab, target.transform.position, Quaternion.identity).GetComponent<StatusEffect>();
			statusEffect.Apply(this, target);
		
			return true;
		}

    }
}
