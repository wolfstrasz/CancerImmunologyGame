using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CellpediaUI
{
	public class PetridishButton : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
	{
		internal static PetridishButton selected = null;
		[SerializeField]
		private AudioSource audioSource = null;
		[SerializeField]
		private Vector3 scaling = new Vector3(1.0f, 1.0f, 1.0f);
		[SerializeField]
		private Image cellImage = null;
		[SerializeField]
		private GameObject glow = null;

		private bool isActivated = false;

		internal void Initialise(Sprite cellSprite)
		{
			cellImage.sprite = cellSprite;
		}
		internal void Activate()
		{
			isActivated = true;
			cellImage.color = Color.white;
		}

		internal void Select()
		{
			
			if (selected != this)
			{
				bool success = Cellpedia.Instance.NextPetridish(this);
				if (success)
				{

					if (selected != null)
					{
						selected.Deselect();
					}

					gameObject.transform.localScale = scaling;
					glow.SetActive(true);
					selected = this;
				}
				return;
			}

			selected = this;
			gameObject.transform.localScale = scaling;
			glow.SetActive(true);

		}

		internal void Deselect()
		{
			gameObject.transform.localScale = Vector3.one;
			glow.SetActive(false);
		}
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
			gameObject.transform.localScale = Vector3.one;
		}

		// When selected.
		public void OnSelect(BaseEventData eventData)
		{
			//if (!isActivated || selected == this) return;
			//Select();
			//audioSource.Play();
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (!isActivated || selected == this) return;
			Select();
			audioSource.Play();
		}
	}
}