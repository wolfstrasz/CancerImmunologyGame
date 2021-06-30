using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ImmunotherapyGame.UI;
using UnityEngine.EventSystems;

namespace ImmunotherapyGame.ResearchAdvancement
{
	[RequireComponent (typeof (Selectable))]
    public class UpgradePurchaseButton : UIMenuNode, ISubmitHandler
    {

		public override void OnCancel(BaseEventData eventData)
		{
			base.OnCancel(eventData);
			ResearchAdvancementSystem.Instance.OnPurchaseButtonCancel();
		
		}

		protected override void OnPointerLeftClick(PointerEventData eventData)
		{
			ResearchAdvancementSystem.Instance.BuyCurrentSelectedUpgrade();
		}

		public void OnSubmit(BaseEventData eventData)
		{
			ResearchAdvancementSystem.Instance.BuyCurrentSelectedUpgrade();
		}
	}
}
