using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ImmunotherapyGame.UI;
using UnityEngine.EventSystems;

namespace ImmunotherapyGame.ImmunotherapyResearchSystem
{
	[RequireComponent (typeof (Selectable))]
    public class UpgradePurchaseButton : UIMenuNode, ISubmitHandler
    {

		public override void OnCancel(BaseEventData eventData)
		{
			base.OnCancel(eventData);
			ImmunotherapyResearch.Instance.OnPurchaseButtonCancel();
		
		}

		protected override void OnPointerLeftClick(PointerEventData eventData)
		{
			ImmunotherapyResearch.Instance.BuyCurrentSelectedUpgrade();
		}

		public void OnSubmit(BaseEventData eventData)
		{
			ImmunotherapyResearch.Instance.BuyCurrentSelectedUpgrade();
		}
	}
}
