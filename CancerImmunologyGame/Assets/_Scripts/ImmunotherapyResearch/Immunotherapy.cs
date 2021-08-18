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
			Debug.Log("IMMUNOTHERAPY UPDATE");
			if (lifetimeLeft > 0f)
			{
				Debug.Log("IMMUNOTHERAPY LIFETIME DROP");

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

			GlobalLevelData.Immunotherapies.Add(this);
		}

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
			Debug.Log("IMMUNOTHERAPY: ACTIVATED");
		}

		public void RemoveImmunotherapyEffects()
		{
			if (!isImmunotherapyApplied) return;

			// Do deactivations
			for (int i = 0; i < applicableUpgrades.Count; ++i)
			{
				applicableUpgrades[i].RemoveEffect();
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

			if (isImmunotherapyApplied)
			{
				RemoveImmunotherapyEffects();
			}
			GlobalLevelData.Immunotherapies.Remove(this);
		}

	}
}
