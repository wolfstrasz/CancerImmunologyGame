using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using ImmunotherapyGame.UI;
using ImmunotherapyGame.Abilities;

namespace ImmunotherapyGame.Player
{
	// TODO: optimise use of caster (maybe by caching caster)
	[RequireComponent (typeof(Selectable))]
    public class ImmunotherapyButton : UIMenuNode
    {
		[SerializeField] protected Image thumbnail = null;
		[SerializeField] protected Animator animator = null;
		[SerializeField] protected PlayerData playerData = null;
		[SerializeField] protected InterfaceControlPanel abilitySwitcherPanel = null;
		[Header("Debug Only")]
		[SerializeField] protected ImmunotherapyCaster abilityCaster = null;

		protected override void OnEnable()
		{
			base.OnEnable();
		
			playerData.onCurrentCasterChanged += OnCasterChanged;
			OnCasterChanged();


			playerData.onCurrentAbilityChanged += OnAbilityChanged;
			OnAbilityChanged();
			
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			playerData.onCurrentCasterChanged -= OnCasterChanged;
			playerData.onCurrentAbilityChanged -= OnAbilityChanged;

			if (abilityCaster != null)
			{
				abilityCaster.onAbilityCasted -= OnAbilityCasted;
				abilityCaster.onCooldownEnded -= OnCooldownFinished;
				abilityCaster = null;
			}
			
		}

		// Player Data callbacks
		private void OnAbilityChanged()
		{
			if (playerData.CurrentAbility != null)
			{
				thumbnail.sprite = playerData.CurrentAbility.thumbnail;
			}
		}

		private void OnCasterChanged()
		{
			ClearCaster();
			SetCaster();
		}

		private void SetCaster()
		{
			if (playerData.CurrentCaster != null)
			{
				abilityCaster = playerData.CurrentCaster;
				abilityCaster.onAbilityCasted += OnAbilityCasted;
				abilityCaster.onCooldownEnded += OnCooldownFinished;

				if (abilityCaster.IsOnCooldown)
				{
					OnAbilityCasted();
				}
				else
				{
					OnCooldownFinished();
				}
			}

		}

		private void ClearCaster()
		{
			if (abilityCaster != null)
			{
				abilityCaster.onAbilityCasted -= OnAbilityCasted;
				abilityCaster.onCooldownEnded -= OnCooldownFinished;
				abilityCaster = null;
			}
		}

		// Caster callbacks
		private void OnCooldownFinished()
		{
			animator.SetTrigger("AbilityReady");
		}

		private void OnAbilityCasted()
		{
			animator.SetTrigger("AbilityCasted");
		}


		// UI Menu node button functionality overrides
		protected override void OnPointerLeftClick(PointerEventData eventData)
		{
			base.OnPointerLeftClick(eventData);

			if (abilityCaster && !abilityCaster.IsOnCooldown)
			{
				abilityCaster.CastAbility();
			}
		}

		protected override void OnPointerRightClick(PointerEventData eventData)
		{
			base.OnPointerRightClick(eventData);

			abilitySwitcherPanel.Open();
		}

		public override void OnPointerEnter(PointerEventData eventData)
		{
			// We dont want selection
			// base.OnPointerEnter(eventData);
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			// base.OnPointerExit(eventData);
		}


	}
}
