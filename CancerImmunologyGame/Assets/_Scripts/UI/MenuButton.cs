using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using ImmunotherapyGame.Audio;

namespace ImmunotherapyGame.UI
{
	public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ISelectHandler, IDeselectHandler, ICancelHandler
	{
		[SerializeField]
		private GameObject selectedView = null;
		[SerializeField]
		private UIAudioClipKey audioClipKey = UIAudioClipKey.NONE;


		private void ApplySelect()
		{
			selectedView.SetActive(true);
			AudioManager.Instance.PlayUISoundClip(audioClipKey, gameObject);
		}

		private void ApplyDeselect()
		{
			selectedView.SetActive(false);
		}

		// When highlighted with mouse.
		public virtual void OnPointerEnter(PointerEventData eventData)
		{
			EventSystem.current.SetSelectedGameObject(gameObject);
		}

		public virtual void OnPointerExit(PointerEventData eventData)
		{
			OnDeselect(eventData);
		}

		// When selected.
		public virtual void OnPointerClick(PointerEventData eventData)
		{
			//// Do something.
			//audioSource.Play();
			//gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		}

		public void OnSelect(BaseEventData eventData)
		{
			ApplySelect();
		}

		public void OnDeselect(BaseEventData eventData)
		{
			ApplyDeselect();
		}

		public void OnCancel(BaseEventData eventData)
		{
			ApplyDeselect();
		}
	}
}