using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ImmunotherapyGame.Abilities;

namespace ImmunotherapyGame.Player
{
    public class ImmunotherapyCaster : AbilityCaster
    {
		[Header("Immunotherapy Caster")]
		[SerializeField] protected PlayerData playerData = null;

		protected override void OnEnable()
		{
			base.OnEnable();
			playerData.onCurrentAbilityChanged += OnAbilityChanged;
			OnAbilityChanged();
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			playerData.onCurrentAbilityChanged -= OnAbilityChanged;
		}

		private void OnAbilityChanged()
		{
			if (playerData.CurrentAbility != null)
			{
				ability = playerData.CurrentAbility;

				if (audioSource)
				{
					audioSource.clip = ability.audioClip;
				}
			}
		}

		public override float CastAbility()
		{
			Debug.Log("Cast Immunotherapy Ability");
			//ability.CastAbility(this.gameObject, target);
			currentCooldown = ability.CooldownTime;

			if (audioSource)
			{
				audioSource.Stop();
				audioSource.Play();
			}

			return 0f;
		}
	}
}
