using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ImmunotherapyGame.Audio;
using ImmunotherapyGame.Core;
using TMPro;

namespace ImmunotherapyGame.UI
{
	[System.Serializable]
    public class MenuDropdownItem : UIMenuNode, IPointerClickHandler, IEventSystemHandler, ISubmitHandler, ICancelHandler, IMoveHandler, IPointerEnterHandler, ISelectHandler, IDeselectHandler
	{
		[Header("Attributes")]
		[SerializeField]
		private TMP_Text itemText = null;

		internal MenuDropdown owner { get; set; }
		internal string ItemName { get => itemText.text; set => itemText.text = value; }
		internal int ItemValue { get; set; }

		private List<GameObject> dropdownItems => owner.dropdownItems;

		public void OnMove(AxisEventData eventData)
		{
			Debug.Log(gameObject + ": OnMove (" + eventData.moveVector);

			if (eventData.moveDir == MoveDirection.Up)
			{
				if (ItemValue > 0)
				{
					EventSystem.current.SetSelectedGameObject(dropdownItems[ItemValue - 1]);
				}
				else
				{
					EventSystem.current.SetSelectedGameObject(dropdownItems[owner.dropdownItems.Count - 1]);
				}
			}

			if (eventData.moveDir == MoveDirection.Down)
			{
				if (ItemValue < owner.dropdownItems.Count - 1)
				{
					EventSystem.current.SetSelectedGameObject(dropdownItems[ItemValue + 1]);
				}
				else
				{
					EventSystem.current.SetSelectedGameObject(dropdownItems[0]);
				}
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			Debug.Log(gameObject + ": OnPointerEnter");

			EventSystem.current.SetSelectedGameObject(gameObject);
		}

		public void OnSelect(BaseEventData eventData)
		{
			Debug.Log(gameObject + ": OnSelect");
			
			foreach (var obj in viewObjectsOnSelect)
			{
				obj.SetActive(true);
			}
		}


		public void OnDeselect(BaseEventData eventData)
		{
			Debug.Log(gameObject + ": OnDeselect");
			
			foreach (var obj in viewObjectsOnSelect)
			{
				obj.SetActive(false);
			}
		}

		public void OnPointerClick(PointerEventData eventData) => OnSubmit(eventData);

		public void OnSubmit(BaseEventData eventData)
		{
			Debug.Log(gameObject + "OnSubmit");

			owner.OnItemSubmitted(this);
			EventSystem.current.SetSelectedGameObject(owner.gameObject);
			owner.view.SetActive(false);

		}

		public void OnCancel(BaseEventData eventData)
		{
			Debug.Log(gameObject + "OnCancel");

			EventSystem.current.SetSelectedGameObject(owner.gameObject);
			owner.view.SetActive(false);
		}

	
	}
}
