using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using ImmunotherapyGame.Audio;

namespace ImmunotherapyGame.UI
{
	[RequireComponent(typeof(Button))]
	public class MenuButton : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IDeselectHandler
	{
		[Header("OnSelect")]
		[SerializeField]
		private List<GameObject> viewObjectsOnSelect = null;
		[SerializeField]
		private UIAudioClipKey audioClipKey = UIAudioClipKey.BUTTON;

		private bool OnSelectView
		{
			set
			{
				foreach (GameObject obj in viewObjectsOnSelect)
				{
					obj.SetActive(value);
				}
			}
		}

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