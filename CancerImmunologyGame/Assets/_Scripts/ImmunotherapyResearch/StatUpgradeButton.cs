using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ImmunotherapyGame.UI;
using UnityEngine.EventSystems;
using ImmunotherapyGame.Audio;

namespace ImmunotherapyGame.ImmunotherapyResearchSystem
{
	[RequireComponent (typeof (Selectable))]
    public class StatUpgradeButton : UIMenuNode, IMoveHandler, ISubmitHandler
    {
		[SerializeField] internal StatUpgrade statUpgrade = null;
		[SerializeField] private GameObject lockedIcon = null;
		[SerializeField] private GameObject statIcon = null;
		[SerializeField] private Image statThumbnail = null;
		[SerializeField] private Image generalThumbnail= null;

		internal bool IsUnlocked => statUpgrade.unlocked;

		internal void Initialise()
		{
			statThumbnail.sprite = statUpgrade.statThumbnailSprite;
			generalThumbnail.sprite = statUpgrade.generalThumbnailSprite;
		}

		internal void UpdateDisplay()
		{
			bool isUnlocked = statUpgrade.unlocked;

			lockedIcon.SetActive(!isUnlocked);
			statIcon.SetActive(isUnlocked);
		}

		public override void OnPointerClick(PointerEventData eventData)
		{
			ImmunotherapyResearch.Instance.SelectStatUpgrade(this);
		}

		// Controller Navigation
		public void OnMove(AxisEventData eventData)
		{
			// TODO: Move Navigation
		}

		public void OnSubmit(BaseEventData eventData)
		{
			ImmunotherapyResearch.Instance.SelectStatUpgrade(this);
		}
	}
}
