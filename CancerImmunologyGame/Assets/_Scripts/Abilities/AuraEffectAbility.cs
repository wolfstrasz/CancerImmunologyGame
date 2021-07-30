using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ImmunotherapyGame.Core;
namespace ImmunotherapyGame.Abilities
{
	[CreateAssetMenu(menuName = "MyAssets/Abilities/Aura Effect Ability")]

	public class AuraEffectAbility : Ability
    {
		[Expandable] public StatAttribute auraRange;

		[SerializeField] private GameObject auraEffectPrefab;
		[SerializeField] private bool isAuraEffectOverTime;


		public float AuraRange => (auraRange != null ? auraRange.CurrentValue : 0f);
		public bool IsAuraEffectOverTime => isAuraEffectOverTime;

		public override bool CastAbility(GameObject abilityCaster, GameObject target )
		{

			AuraEffect auraEffect = Instantiate(auraEffectPrefab, target.transform.position, Quaternion.identity).GetComponent<AuraEffect>();
			auraEffect.Apply(this, target);

			return true;
		}

	}
}
