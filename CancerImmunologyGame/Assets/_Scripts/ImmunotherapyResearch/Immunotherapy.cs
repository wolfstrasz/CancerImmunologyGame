using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using ImmunotherapyGame.Core;

namespace ImmunotherapyGame.ImmunotherapyResearchSystem
{
    public class Immunotherapy : Singleton<Immunotherapy>
    {
		[Header("Data")]
		[SerializeField] List<StatUpgrade> applicableUpgrades = null;
		[SerializeField] [Expandable] private StatAttribute lifetimeStat = null;
		[SerializeField] [Expandable] private StatAttribute cooldownStat = null;

		[Header ("UI Links")]
		[SerializeField] private ResourceBar cooldownBar = null;
		[SerializeField] private ResourceBar lifetimeBar = null;
		[SerializeField] private ImmunotherapyButton button = null;

		[Header ("Debug")]
		[SerializeField] [ReadOnly] float lifetimeLeft = 0f;
		[SerializeField] [ReadOnly] float cooldownLeft = 0f;
		[SerializeField] [ReadOnly] PlayerControls controls = null;

		[SerializeField] [ReadOnly] private bool isImmunotherapyApplied = false;

		public void OnUpdate()
		{
			// If immunotherapy was applied wait until it finishes then reverse the effects.
			if (lifetimeLeft > 0f)
			{
				lifetimeLeft -= Time.deltaTime;

				if (lifetimeLeft <= 0f)
				{
					lifetimeLeft = 0f;
					if (isImmunotherapyApplied)
					{
						RemoveImmunotherapyEffects();
					}
				}

				lifetimeBar.SetValue(lifetimeLeft);

			}
			// If the immunotherapy has finished and is on cooldown wait until it can be activated again.
			else if (cooldownLeft > 0f)
			{
				cooldownLeft -= Time.deltaTime;

				if (cooldownLeft <= 0f)
				{
					cooldownLeft = 0f;
					button.Activate();
				}

				cooldownBar.SetInverseValue(cooldownLeft);

			}
		}

		/// <summary>
		/// Updates the player UI to display the lifetime of the immunotherapy.
		/// </summary>
		private void SwitchToLifetimeBar()
		{
			cooldownBar.gameObject.SetActive(false);
			lifetimeBar.gameObject.SetActive(true);
			lifetimeLeft = lifetimeStat.CurrentValue;
			lifetimeBar.SetMaxValue(lifetimeStat.CurrentValue);
			lifetimeBar.SetValue(lifetimeLeft);
			button.Deactivate();
		}

		/// <summary>
		/// Updates the player UI to display the cooldown of the immunotherapy.
		/// </summary>
		private void SwitchToCooldownBar()
		{
			cooldownBar.gameObject.SetActive(true);
			lifetimeBar.gameObject.SetActive(false);
			cooldownLeft = cooldownStat.CurrentValue;
			cooldownBar.SetMaxValue(cooldownStat.CurrentValue);
			cooldownBar.SetInverseValue(cooldownLeft);
		}

		private void OnEnable()
		{
			Debug.Log("Attach Activate Immunotherapy to  Immunotherapy Started");
			controls = new PlayerControls();
			controls.Enable();
			controls.Gameplay.ActivateImmunotherapy.started += ActivateImmunotherapy;

			cooldownBar.SetMaxValue(cooldownStat.CurrentValue);
			cooldownBar.SetInverseValue(cooldownLeft);

			lifetimeBar.SetMaxValue(lifetimeStat.CurrentValue);
			lifetimeBar.SetValue(lifetimeLeft);

			cooldownBar.gameObject.SetActive(true);
			lifetimeBar.gameObject.SetActive(false);
			button.Activate();

			GlobalLevelData.Immunotherapies.Add(this);
		}

		/// <summary>
		/// Goes through the applicable upgrades and applies them.
		/// </summary>
		private void ActivateImmunotherapyEffects()
		{
			// Do activations
			// .....
			for (int i = 0; i < applicableUpgrades.Count; ++i)
			{
				applicableUpgrades[i].ApplyEffect();
			}

			isImmunotherapyApplied = true;
			SwitchToLifetimeBar();
		}

		/// <summary>
		/// Goes through the applicable upgrades and undoes their effects.
		/// </summary>
		public void RemoveImmunotherapyEffects()
		{

			// Do deactivations
			for (int i = 0; i < applicableUpgrades.Count; ++i)
			{
				applicableUpgrades[i].RemoveEffect();
			}

			isImmunotherapyApplied = false;
			SwitchToCooldownBar();

		}

	
		private void ActivateImmunotherapy(InputAction.CallbackContext context)
		{
			Debug.Log("Activate Immunotherapy called");
			if (cooldownLeft == 0f && !isImmunotherapyApplied)
			{
				ActivateImmunotherapyEffects();
			}
		}

		public void ActivateImmunotherapy()
		{
			if (cooldownLeft == 0f && !isImmunotherapyApplied)
			{
				ActivateImmunotherapyEffects();
			}
		}

		private void OnDisable()
		{
			controls.Disable();
			controls.Gameplay.ActivateImmunotherapy.started -= ActivateImmunotherapy;
			lifetimeLeft = 0f;
			cooldownLeft = 0f;
			SwitchToCooldownBar();

			if (isImmunotherapyApplied)
			{
				RemoveImmunotherapyEffects();
			}
			GlobalLevelData.Immunotherapies.Remove(this);
		}

	}
}
