using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using ImmunotherapyGame.Audio;

namespace ImmunotherapyGame.UI
{
	[RequireComponent(typeof(Button))]
	public class MenuButton : UIMenuNode, IPointerEnterHandler, ISelectHandler, IDeselectHandler
	{
		// When highlighted with mouse.
		public virtual void OnPointerEnter(PointerEventData eventData)
			=> EventSystem.current.SetSelectedGameObject(gameObject);
		
		public void OnSelect(BaseEventData eventData)
		{
			OnSelectView = true;
			AudioManager.Instance.PlayUISoundClip(audioClipKey, gameObject);
		}

		public void OnDeselect(BaseEventData eventData)
		{
			OnSelectView = false;
		}
	}
}