using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class MenuButton : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField]
	private AudioSource audioSource = null;
	[SerializeField]
	private Vector3 scaling = new Vector3(1.0f, 1.0f, 1.0f);

	// When highlighted with mouse.
	public void OnPointerEnter(PointerEventData eventData)
	{
		// Do something.
		audioSource.Play();
		gameObject.transform.localScale = scaling;
		Debug.Log("<color=red>Event:</color> Completed mouse highlight.");
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
	}

	// When selected.
	public void OnSelect(BaseEventData eventData)
	{
		// Do something.
		audioSource.Play();
		gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		Debug.Log("<color=red>Event:</color> Completed selection.");
	}
}