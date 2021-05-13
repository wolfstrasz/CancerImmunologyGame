using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using ImmunotherapyGame.Audio;

namespace ImmunotherapyGame.UI
{
    [RequireComponent(typeof(Slider))]
    public class MenuSlider : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler, ISubmitHandler
    {
        private Slider slider = null;
        [SerializeField]
        private List<GameObject> onSelectActivateObjects = new List<GameObject>();
        [SerializeField]
        private UIAudioClipKey audioClipKey = UIAudioClipKey.BUTTON;

        void Awake()
		{
            slider = GetComponent<Slider>();
            slider.onValueChanged.AddListener(delegate { OnValueChanged(); });
		}

        private void OnValueChanged()
		{
            AudioManager.Instance.PlayUISoundClip(audioClipKey, this.gameObject);
        }

        public void OnDeselect(BaseEventData eventData)
		{
            foreach(var obj in onSelectActivateObjects)
			{
                obj.SetActive(false);
			}
            Debug.Log(gameObject + ": OnDeselect");

        }

        public void OnSelect(BaseEventData eventData)
		{
            foreach (var obj in onSelectActivateObjects)
            {
                obj.SetActive(true);
            }
            AudioManager.Instance.PlayUISoundClip(audioClipKey, this.gameObject);
            Debug.Log(gameObject + ": OnSelect");

        }

        public void OnPointerEnter(PointerEventData eventData)
		{
            OnSelect(eventData);
		}

        public void OnPointerExit (PointerEventData eventData)
		{
            OnDeselect(eventData);
		}

		public void OnSubmit(BaseEventData eventData)
		{
            Debug.Log(gameObject + ": OnSubmit");
            foreach (var obj in onSelectActivateObjects)
            {
                obj.SetActive(true);
            }
        }
	}
}
