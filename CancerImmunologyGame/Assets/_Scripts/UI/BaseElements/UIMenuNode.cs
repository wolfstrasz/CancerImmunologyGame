using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using ImmunotherapyGame.Audio;

namespace ImmunotherapyGame.UI
{
	public abstract class UIMenuNode : MonoBehaviour, ICancelHandler, IDeselectHandler, ISelectHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
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
				if (viewObjectsOnSelect != null)
				{
					foreach (GameObject obj in viewObjectsOnSelect)
					{
						obj.SetActive(value);
					}
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
			Debug.Log("UMN: DESELECT -> " + gameObject.name);

			OnSelectView = false;
		}

		public virtual void OnSelect(BaseEventData eventData)
		{
			Debug.Log("UMN: SELECT -> " + gameObject.name);
			OnSelectView = true;
			AudioManager.Instance.PlayUISoundClip(audioClipKey, gameObject);
		}

		// When highlighted with mouse.
		public virtual void OnPointerEnter(PointerEventData eventData)
		{
			Debug.Log("UMN: POINTER_ENTER -> " + gameObject.name);
			EventSystem.current.SetSelectedGameObject(gameObject);

		}


		public virtual void OnPointerExit(PointerEventData eventData) 
		{
			Debug.Log("UMN: POINTER_EXIT -> " + gameObject.name);
		}


		protected virtual void OnDisable()
		{
			OnSelectView = false;
		}

		protected virtual void OnEnable()
		{

		}

		public virtual void OnPointerClick(PointerEventData eventData)
		{
			switch (eventData.button)
			{
				case PointerEventData.InputButton.Left: OnPointerLeftClick(eventData); break;
				case PointerEventData.InputButton.Right: OnPointerRightClick(eventData); break;
				case PointerEventData.InputButton.Middle: OnPointerMiddleClick(eventData); break;
				default: OnPointerLeftClick(eventData); break;
			}
		}

		protected virtual void OnPointerLeftClick(PointerEventData eventData) { }
		protected virtual void OnPointerRightClick(PointerEventData eventData) { }
		protected virtual void OnPointerMiddleClick(PointerEventData eventData) { } 

	}
}
