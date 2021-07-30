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
		[SerializeField] ImmunotherapyResearchData data = null;
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

		public void Activate()
		{
			gameObject.SetActive(true);
		}

		public void OnUpdate()
		{
			if (!gameObject.activeInHierarchy) return;

			if (lifetimeLeft > 0f)
			{
				lifetimeLeft -= Time.deltaTime;

				if (lifetimeLeft <= 0f)
				{
					lifetimeLeft = 0f;
					RemoveImmunotherapyEffects();
				}

				lifetimeBar.SetValue(lifetimeLeft);

			}

			else if (cooldownLeft > 0f)
			{
				cooldownLeft -= Time.deltaTime;

				if (cooldownLeft <= 0f)
				{
					cooldownLeft = 0f;
					button.Activate();
					Debug.Log("IMMUNOTHERAPY: AVAILABLE");

				}

				cooldownBar.SetInverseValue(cooldownLeft);

			}
			//if (abilityCaster != null)
			//{
			//	abilityCooldownBar.SetValue(abilityCaster.CooldownTimePassed);
			//}
		}

		private void Update()
		{
			OnUpdate();
		}


		private void SwitchToLifetimeBar()
		{
			cooldownBar.gameObject.SetActive(false);
			lifetimeBar.gameObject.SetActive(true);
			lifetimeLeft = lifetimeStat.CurrentValue;
			lifetimeBar.SetValue(lifetimeLeft);
			button.Deactivate();
		}

		private void SwitchToCooldownBar()
		{
			cooldownBar.gameObject.SetActive(true);
			lifetimeBar.gameObject.SetActive(false);
			cooldownLeft = cooldownStat.CurrentValue;
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
		}

		private void ActivateImmunotherapyEffects()
		{
			// Do activations
			// .....
			for (int i = 0; i < data.statUpgrades.Count; ++i)
			{
				data.statUpgrades[i].ApplyEffect();
			}

			SwitchToLifetimeBar();
			Debug.Log("IMMUNOTHERAPY: ACTIVATED");
		}

		public void RemoveImmunotherapyEffects()
		{ 
			// Do deactivations
			for (int i = 0; i < data.statUpgrades.Count; ++i)
			{
				data.statUpgrades[i].RemoveEffect();
			}

			SwitchToCooldownBar();
			Debug.Log("IMMUNOTHERAPY: FINISHED");
		}

	
		private void ActivateImmunotherapy(InputAction.CallbackContext context)
		{
			Debug.Log("Activate Immunotherapy called");
			if (cooldownLeft == 0f)
			{
				ActivateImmunotherapyEffects();
			}
		}

		public void ActivateImmunotherapy()
		{
			if (cooldownLeft == 0f)
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
		}

	}
}