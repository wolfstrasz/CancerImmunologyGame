using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using ImmunotherapyGame.Core;

namespace ImmunotherapyGame.Abilities
{
    [CreateAssetMenu(menuName = "Abilities/Status Effect Ability")]
    public class StatusEffectAbility : Ability
    {
		[Expandable] public StatAttribute statusEffectLifetime;
		[SerializeField] private GameObject statusEffectPrefab;
		[SerializeField] private bool isStatusEffectOverTime;


		public bool IsStatusEffectOverTime => isStatusEffectOverTime;
		public float StatusEffectLifetime => (statusEffectLifetime != null ? statusEffectLifetime.CurrentValue : 0f);


		public override bool CastAbility(GameObject abilityCaster, GameObject target)
		{

			StatusEffect statusEffect = Instantiate(statusEffectPrefab, target.transform.position, Quaternion.identity).GetComponent<StatusEffect>();
			statusEffect.Apply(this, target);
		
			return true;
		}

    }
}
