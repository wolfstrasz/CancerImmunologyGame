using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ImmunotherapyGame.UI;
using UnityEngine.EventSystems;

namespace ImmunotherapyGame.ResearchAdvancement
{
	[RequireComponent (typeof (Selectable))]
    public class UpgradePurchaseButton : UIMenuNode
    {

		public override void OnCancel(BaseEventData eventData)
		{
			base.OnCancel(eventData);
			EventSystem.current.SetSelectedGameObject(
				ResearchAdvancementSystem.Instance.currentSelectedStatUpgradeButton
				);
		}


		protected override void OnPointerLeftClick(PointerEventData eventData)
		{
			
		}


	}
}
