using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using ImmunotherapyGame.Core;
using ImmunotherapyGame.Audio;

using TMPro;

namespace ImmunotherapyGame.UI
{
    public class MenuDropdown : Selectable, IPointerClickHandler, IEventSystemHandler, ISubmitHandler, ICancelHandler
    {
        [Header("Menu")]
        [SerializeField]
        private List<GameObject> onSelectedActivateGameObjects = null;
        [SerializeField]
        private UIAudioClipKey audioClipKey = UIAudioClipKey.BUTTON;

        [Header("Dropdown")]
        [SerializeField]
        internal GameObject dropdownView = null;
        [SerializeField]
        private Transform content = null;
        [SerializeField]
        private GameObject templateObject = null;
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
            dropdownView.SetActive(false);
            EventSystem.current.SetSelectedGameObject(gameObject);
		}

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log(gameObject + ": OnPointerClick");
        }

        public void OnSubmit(BaseEventData eventData)
		{
            Debug.Log(gameObject + ": OnSubmit");

            dropdownView.SetActive(true);
            EventSystem.current.SetSelectedGameObject(selectedItem.gameObject);

            foreach (var obj in onSelectedActivateGameObjects)
            {
                obj.SetActive(true);
            }
        }

        public void OnCancel(BaseEventData eventData)
        {
            Debug.Log(gameObject + ": OnCancel");

            dropdownView.SetActive(false);
        }


#region Selectable Overrides
        public override void OnPointerEnter(PointerEventData eventData)
		{
            base.OnPointerEnter(eventData);
            Debug.Log(gameObject + ": OnPointerEnter");

            OnSelect(eventData);
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
            base.OnPointerExit(eventData);
            Debug.Log(gameObject + ": OnPointerExit");

			OnDeselect(eventData);
		}

		public override void OnDeselect(BaseEventData eventData)
		{
            base.OnDeselect(eventData);
            Debug.Log(gameObject + ": OnDeselect");

           
            foreach (var obj in onSelectedActivateGameObjects)
			{
                obj.SetActive(false);
			}

           // dropdownView.SetActive(false);
        }

		public override void OnSelect(BaseEventData eventData)
		{
            base.OnSelect(eventData);
            Debug.Log(gameObject + ": OnSelect");

            foreach (var obj in onSelectedActivateGameObjects)
            {
                obj.SetActive(true);
            }
            AudioManager.Instance.PlayUISoundClip(audioClipKey, this.gameObject);
        }
#endregion

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
                MenuDropdownItem newItem = Instantiate(templateObject, content).GetComponent<MenuDropdownItem>();
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
            } else
			{
                selectedDisplayText.text = selectedItem.ItemName;
			}
		}
#endregion

	}
}
