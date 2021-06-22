using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ImmunotherapyGame.UI;
using UnityEngine.EventSystems;
using ImmunotherapyGame.Audio;

namespace ImmunotherapyGame.ResearchAdvancement
{
	[RequireComponent (typeof (Selectable))]
    public class StatUpgradeButton : UIMenuNode, IMoveHandler, ISubmitHandler
    {
		[SerializeField] internal StatUpgrade statUpgrade = null;

		[SerializeField] private GameObject lockedIcon = null;
		[SerializeField] private GameObject statIcon = null;

		public void UpdateDisplay()
		{
			bool isUnlocked = statUpgrade.unlocked;

			lockedIcon.SetActive(!isUnlocked);
			statIcon.SetActive(isUnlocked);

		}

		// Mouse navigation
		public override void OnPointerEnter(PointerEventData eventData)
		{
			OnSelectView = true;
			AudioManager.Instance.PlayUISoundClip(audioClipKey, gameObject);
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			OnSelectView = false;
		}

		public override void OnPointerClick(PointerEventData eventData)
		{
			ResearchAdvancementSystem.Instance.SelectStatUpgrade(this);
		}

		// Controller Navigation
		public void OnMove(AxisEventData eventData)
		{
			// TODO: Move Navigation
		}

		public void OnSubmit(BaseEventData eventData)
		{
			ResearchAdvancementSystem.Instance.SelectStatUpgrade(this);
		}
	}
}
