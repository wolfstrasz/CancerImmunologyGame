using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using ImmunotherapyGame.Core;
using ImmunotherapyGame.Audio;

using TMPro;

namespace ImmunotherapyGame.UI
{
    [RequireComponent(typeof(Selectable))]
    public class MenuDropdown : UIMenuNode, IPointerClickHandler, ISubmitHandler, IPointerExitHandler
    {
        [Header("Dropdown")]
        [SerializeField]
        internal GameObject view = null;
        [SerializeField]
        private Transform content = null;
        [SerializeField]
        private GameObject templateItem = null;
        internal List<GameObject> dropdownItems = null;

        [Header("Selected Item")]
        [SerializeField]
        private TMP_Text selectedDisplayText = null;
        [ReadOnly]
        private int initialValue = -1;
        [ReadOnly]
        private MenuDropdownItem selectedItem = null;


        // Accessors
        public int CurrentValue { get => selectedItem.ItemValue; set => selectedItem = dropdownItems[value].GetComponent<MenuDropdownItem>(); }
        public string CurrentValueName { get => selectedItem.ItemName; }

		// Events
		public delegate void OnValueChanged();
        public OnValueChanged onValueChanged;

        internal void OnItemSubmitted(MenuDropdownItem item)
		{
            if (selectedItem != item)
            {
                Debug.Log("New item selected");
                selectedItem = item;
                RefreshShowValue();
                onValueChanged();
            }
            else
			{
                Debug.Log("Same value selected");
			}
            view.SetActive(false);
            EventSystem.current.SetSelectedGameObject(gameObject);
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			base.OnPointerExit(eventData);
            view.SetActive(false);
		}
		public void OnPointerClick(PointerEventData eventData)
		{
            Debug.Log("MENUDROPDOWN CLICK");
            OnSubmit(eventData);
        }

        public void OnSubmit(BaseEventData eventData)
		{
            if (view.activeInHierarchy)
            {
                view.SetActive(false);

                return;
            }
            Debug.Log("MENUDROPDOWN SUBMIT");

            view.SetActive(true);
            EventSystem.current.SetSelectedGameObject(selectedItem.gameObject);
        }

		#region Menu Dropdown Functionality
		public void ClearOptions()
		{
            if (dropdownItems != null)
			{
                dropdownItems.Clear();
			}
		}

        public void AddOptions(List<string> options)
		{
            initialValue = options.Count - 1;
            dropdownItems = new List< GameObject>(options.Count);

            for (int i = 0; i < options.Count; ++i)
			{
                MenuDropdownItem newItem = Instantiate(templateItem, content).GetComponent<MenuDropdownItem>();
                newItem.ItemName = options[i];
                newItem.ItemValue = i;
                newItem.owner = this;
                newItem.gameObject.SetActive(true);
                newItem.gameObject.name = "Item " + i.ToString() + ": " + options;
                dropdownItems.Add(newItem.gameObject);
            }
		}

        public void RefreshShowValue()
        {
            if (selectedItem == null)
            {
                if (initialValue >= 0 && initialValue < dropdownItems.Count)
                {
                    selectedItem = dropdownItems[initialValue].GetComponent<MenuDropdownItem>();
                    selectedDisplayText.text = selectedItem.ItemName;
                }
            } 
            else
			{
                selectedDisplayText.text = selectedItem.ItemName;
			}
		}
#endregion

	}
}
