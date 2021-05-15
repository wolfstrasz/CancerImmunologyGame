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
    public class MenuDropdownItem : UIMenuNode, IEventSystemHandler, IPointerClickHandler, ISubmitHandler, IMoveHandler
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

		public void OnPointerClick(PointerEventData eventData)
		{
			Debug.Log("ITEM: CLICK");
			OnSubmit(eventData);

		}

		public void OnSubmit(BaseEventData eventData)
		{
			Debug.Log("->>>>>>>>>>>>>>>>>>>>>Item submitted");
			owner.OnItemSubmitted(this);
			EventSystem.current.SetSelectedGameObject(owner.gameObject);
			owner.view.SetActive(false);

		}

		public override void OnCancel(BaseEventData eventData)
		{
			EventSystem.current.SetSelectedGameObject(owner.gameObject);
			owner.view.SetActive(false);
			base.OnCancel(eventData);
		}



	}
}
