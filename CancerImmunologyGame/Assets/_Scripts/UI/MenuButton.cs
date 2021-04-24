using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ImmunotherapyGame
{
	public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
	{
		[Header("Menu Button Attributes")]
		[SerializeField]
		private AudioSource audioSource = null;
		[SerializeField]
		private Vector3 scaling = new Vector3(1.0f, 1.0f, 1.0f);

		// When highlighted with mouse.
		public virtual void OnPointerEnter(PointerEventData eventData)
		{
			// Do something.
			audioSource.Play();
			gameObject.transform.localScale = scaling;
		}

		public virtual void OnPointerExit(PointerEventData eventData)
		{
			gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		}

		// When selected.
		public virtual void OnPointerClick(PointerEventData eventData)
		{
			// Do something.
			audioSource.Play();
			gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		}

	}
}