using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using ImmunotherapyGame.Audio;

namespace ImmunotherapyGame.UI
{
	public abstract class UIMenuNode : MonoBehaviour, ICancelHandler, IDeselectHandler, ISelectHandler, IPointerEnterHandler, IPointerExitHandler
	{
		[Header("OnSelect")]
		[SerializeField]
		protected List<GameObject> viewObjectsOnSelect = null;
		[SerializeField]
		protected UIAudioClipKey audioClipKey = UIAudioClipKey.BUTTON;

		// Events
		public delegate void OnCancelCalled();
		public OnCancelCalled onCancelCall;

		public bool OnSelectView
		{
			set
			{
				foreach (GameObject obj in viewObjectsOnSelect)
				{
					obj.SetActive(value);
				}
			}
		}

		public virtual void OnCancel(BaseEventData eventData)
		{
			OnDeselect(eventData);
			Debug.Log("ON CANCEL CALL: " + gameObject.name);
			if (onCancelCall != null)
				onCancelCall();
		}

		public virtual void OnDeselect(BaseEventData eventData)
		{
			OnSelectView = false;
		}

		public virtual void OnSelect(BaseEventData eventData)
		{
			OnSelectView = true;
			AudioManager.Instance.PlayUISoundClip(audioClipKey, gameObject);
		}

		// When highlighted with mouse.
		public virtual void OnPointerEnter(PointerEventData eventData)
			=> EventSystem.current.SetSelectedGameObject(gameObject);


		public virtual void OnPointerExit(PointerEventData eventData)
			=> OnDeselect(eventData);


		protected virtual void OnDisable()
		{
			OnSelectView = false;
		}

	}
}
