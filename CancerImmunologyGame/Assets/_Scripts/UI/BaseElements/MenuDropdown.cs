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
    public class MenuDropdown : UIMenuNode, ISubmitHandler, IPointerExitHandler
    {
        [Header("Dropdown")]
        [SerializeField]
        internal GameObject view = null;
        [SerializeField]
        private Transform content = null;
        [SerializeField]
        private GameObject templateItem = null;
        internal List<MenuDropdownItem> dropdownItems = null;

        [Header("Selected Item")]
        [SerializeField]
        private TMP_Text selectedDisplayText = null;
        [ReadOnly]
        private int possibleInitialValue = -1;
        [ReadOnly]
        private MenuDropdownItem selectedItem = null;


        // Accessors
        public int CurrentValue 
        { 
            get => selectedItem.ItemValue; 
            set 
            {
                if (selectedItem != null)
                    selectedItem.SilentDeselect();
                selectedItem = dropdownItems[value];
                RefreshShownValue(); 
            } 
        }

        internal void OnItemSubmitted(MenuDropdownItem item)
		{
            if (selectedItem != item)
            {
                selectedItem = item;
                RefreshShownValue();
            }
            else
			{
			}
            view.SetActive(false);
            EventSystem.current.SetSelectedGameObject(gameObject);
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
            Debug.Log("UMN: POINTER_EXIT -> " + gameObject.name);

            OnDeselect(eventData);
            foreach (MenuDropdownItem item in dropdownItems)
			{
                item.SilentDeselect();
			}
            view.SetActive(false);
		}


        public void OnSubmit(BaseEventData eventData)
		{
            if (view.activeInHierarchy)
            {
                view.SetActive(false);

                return;
            }

            view.SetActive(true);
            EventSystem.current.SetSelectedGameObject(selectedItem.gameObject);
        }

        protected override void OnPointerLeftClick(PointerEventData eventData)
        {
            OnSubmit(eventData);
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
            possibleInitialValue = options.Count - 1;
            dropdownItems = new List< MenuDropdownItem>(options.Count);

            for (int i = 0; i < options.Count; ++i)
			{
                MenuDropdownItem newItem = Instantiate(templateItem, content).GetComponent<MenuDropdownItem>();
                newItem.ItemName = options[i];
                newItem.ItemValue = i;
                newItem.owner = this;
                newItem.gameObject.SetActive(true);
                newItem.gameObject.name = "Item " + i.ToString() + ": " + options;
                dropdownItems.Add(newItem);
            }

            if (possibleInitialValue > 1)
			{
                selectedItem = dropdownItems[0];
			}
		}

        public void RefreshShownValue()
        {
            selectedDisplayText.text = selectedItem.ItemName;
		}


		#endregion

	}
}
