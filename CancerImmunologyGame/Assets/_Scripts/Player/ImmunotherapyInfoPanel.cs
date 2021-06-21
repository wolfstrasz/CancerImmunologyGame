using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.Abilities;

namespace ImmunotherapyGame.Player
{
    public class ImmunotherapyInfoPanel : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData = null;
		[SerializeField] private ResourceBar abilityCooldownBar = null;
		[Header("Read only")]
		[SerializeField] [ReadOnly] private ImmunotherapyCaster abilityCaster = null;
		[SerializeField] [ReadOnly] private Ability ability = null;

		private void Update()
		{
			if (abilityCaster != null)
			{
				abilityCooldownBar.SetValue(abilityCaster.CooldownTimePassed);
			}
		}

		private void OnEnable()
		{
			playerData.onCurrentCasterChanged += OnCasterChanged;
			playerData.onCurrentAbilityChanged += OnAbilityChanged;
			SetCaster();
			SetAbility();
		}

		private void OnDisable()
		{
			playerData.onCurrentCasterChanged -= OnCasterChanged;
			playerData.onCurrentAbilityChanged -= OnAbilityChanged;
			ClearAbility();
			ClearCaster();
		}


		// Caster callbacks
		private void ClearCaster()
		{
			if (abilityCaster != null)
			{
				abilityCaster.onCooldownEnded -= OnAbilityCooldownFinished;
				abilityCaster = null;
			}
		}

		private void SetCaster()
		{
			if (playerData.CurrentCaster != null)
			{
				abilityCaster = playerData.CurrentCaster;
				abilityCaster.onCooldownEnded += OnAbilityCooldownFinished;
			}
		}

		private void OnCasterChanged()
		{
			ClearCaster();
			SetCaster();
		}

		private void OnAbilityCooldownFinished()
		{
			// TODO: Trigger animation for bar
		}

		// Ability callbacks
		private void OnAbilityChanged()
		{
			ClearAbility();
			SetAbility();
		}

		private void ClearAbility()
		{
			if (ability != null)
			{
				ability.cooldownTime.onValueChanged -= OnAbilityCooldownChanged;
			}
		}

		private void SetAbility()
		{
			if (playerData.CurrentAbility)
			{
				ability = playerData.CurrentAbility;
				ability.cooldownTime.onValueChanged += OnAbilityCooldownChanged;
			}
		}

		private void OnAbilityCooldownChanged()
		{
			abilityCooldownBar.SetMaxValue(ability.CooldownTime);
		}

	}
}
