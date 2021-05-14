using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ImmunotherapyGame.Core;
using TMPro;

namespace ImmunotherapyGame.UI
{
	[System.Serializable]
    public class MenuDropdownItem : Selectable, IPointerClickHandler, IEventSystemHandler, ISubmitHandler, ICancelHandler
	{
		[SerializeField]
		private List<GameObject> onSelectActivateGameObjects = null;
		[SerializeField]
		private TMP_Text itemText = null;

		internal MenuDropdown owner { get; set; }
		internal string ItemName { get => itemText.text; set => itemText.text = value; }
		internal int ItemValue { get; set; }

		private List<GameObject> dropdownItems => owner.dropdownItems;

		public override void OnMove(AxisEventData eventData)
		{
			
			base.OnMove(eventData);
			Debug.Log(gameObject + ": OnMove (" + eventData.moveVector);

			if (eventData.moveDir == MoveDirection.Up)
			{
				if (ItemValue > 1)
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

		public override void OnPointerEnter(PointerEventData eventData)
		{
			base.OnPointerEnter(eventData);
			Debug.Log(gameObject + ": OnPointerEnter");

			EventSystem.current.SetSelectedGameObject(gameObject);
		}

		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			Debug.Log(gameObject + ": OnSelect");
			
			foreach (var obj in onSelectActivateGameObjects)
			{
				obj.SetActive(true);
			}
		}


		public override void OnDeselect(BaseEventData eventData)
		{
			base.OnDeselect(eventData);
			Debug.Log(gameObject + ": OnDeselect");
			
			foreach (var obj in onSelectActivateGameObjects)
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
