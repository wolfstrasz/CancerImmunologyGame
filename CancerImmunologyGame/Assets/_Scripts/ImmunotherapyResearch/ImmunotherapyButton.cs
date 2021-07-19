using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using ImmunotherapyGame.UI;
using ImmunotherapyGame.Abilities;

namespace ImmunotherapyGame.ImmunotherapyResearchSystem
{
	// TODO: optimise use of caster (maybe by caching caster)
	[RequireComponent (typeof(Selectable))]
    public class ImmunotherapyButton : UIMenuNode
    {
		[SerializeField] protected Image thumbnail = null;
		[SerializeField] protected Animator animator = null;

		[SerializeField] [ReadOnly] bool activated = false;

		public void Activate()
		{
			animator.SetBool("Available", true);
			activated = true;
		}

		public void Deactivate()
		{
			animator.SetBool("Available", false);
			activated = false;
		}

		protected override void OnEnable()
		{
			base.OnEnable();			
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			activated = false;
		}

		// UI Menu node button functionality overrides
		protected override void OnPointerLeftClick(PointerEventData eventData)
		{
			if (!activated) return;
			Immunotherapy.Instance.ActivateImmunotherapy();
			base.OnPointerLeftClick(eventData);
		}


		public override void OnPointerEnter(PointerEventData eventData)
		{
			Debug.Log("UMN: POINTER_ENTER -> " + gameObject.name);

			// We dont want selection
			// base.OnPointerEnter(eventData);
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			// base.OnPointerExit(eventData);
			Debug.Log("UMN: POINTER_EXIT -> " + gameObject.name);

		}


	}
}
