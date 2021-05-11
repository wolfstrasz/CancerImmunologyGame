using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using ImmunotherapyGame.Core;

namespace ImmunotherapyGame.CellpediaSystem
{
	
	public class PetridishButton : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
	{
		internal static PetridishButton selected = null;
		[ReadOnly]
		private bool isActivated = false;
		[ReadOnly]
		internal CellpediaObject cellObject = null;

		[Header("Petridish")]
		[SerializeField]
		private Image cellImage = null;


		[Header("Button Functionality")]
		[SerializeField]
		private AudioSource audioSource = null;
		[SerializeField]
		private Vector3 scaling = new Vector3(1.0f, 1.0f, 1.0f);
		[SerializeField]
		private Vector3 initialScaling = new Vector3(1.0f, 1.0f, 1.0f);

		internal void Initialise(CellpediaObject cellObject)
		{
			this.cellObject = cellObject;
			cellImage.sprite = cellObject.sprite;
			cellImage.color = Color.black;
			transform.localScale = initialScaling;
			Deactivate();
		}

		internal void Activate()
		{
			isActivated = true;
			cellImage.color = Color.white;
		}

		internal void Deactivate()
		{
			isActivated = false;
			cellImage.color = Color.black;
		}

		internal void SelectCell()
		{
			
			if (selected != this)
			{
				bool success = Cellpedia.Instance.microscope.NextPetridish(cellObject);
				if (success)
				{

					if (selected != null)
					{
						selected.DeselectCell();
					}

					gameObject.transform.localScale = scaling;
					selected = this;
				}
				return;
			}

			selected = this;
			gameObject.transform.localScale = scaling;

		}

		internal void DeselectCell()
		{
			gameObject.transform.localScale = initialScaling;
		}

		// BUTTON FUNCTIONALITY


		// When highlighted with mouse.
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (!isActivated || selected == this) return;
			gameObject.transform.localScale = scaling;
			audioSource.Play();
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (!isActivated || selected == this ) return;
			gameObject.transform.localScale = initialScaling;
		}

		// When selected.
		public void OnSelect(BaseEventData eventData)
		{
			if (!isActivated || selected == this) return;
			gameObject.transform.localScale = scaling;
			audioSource.Play();
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (!isActivated || selected == this) return;
			SelectCell();
			audioSource.Play();
		}
	}
}