using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using ImmunotherapyGame.Audio;

namespace ImmunotherapyGame.UI
{
    [RequireComponent(typeof(Slider))]
    public class MenuSlider : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler
    {
        [Header("OnSelect")]
        [SerializeField]
        private List<GameObject> viewObjectsOnSelect = null;
        [SerializeField]
        private UIAudioClipKey audioClipKey = UIAudioClipKey.BUTTON;

        private bool OnSelectView 
        { 
            set 
            {
                foreach (GameObject obj in viewObjectsOnSelect)
				{
                    obj.SetActive(value);
				}
			} 
        }

        // Cached objects
        private Slider slider = null;

        // Private methods
        private void OnValueChanged() 
            => AudioManager.Instance.PlayUISoundClip(audioClipKey, this.gameObject);

        // Protected methods
        void Awake() 
            => slider = GetComponent<Slider>();
        void OnEnable() 
            => slider.onValueChanged.AddListener(delegate { OnValueChanged(); });
        void OnDisable() 
            => slider.onValueChanged.RemoveListener(delegate { OnValueChanged(); });

        // Public methods
        public void OnPointerEnter(PointerEventData eventData)
            => EventSystem.current.SetSelectedGameObject(gameObject);

        public void OnDeselect(BaseEventData eventData)
		    => OnSelectView = false;

        public void OnSelect(BaseEventData eventData)
		{
            OnSelectView = true;
            AudioManager.Instance.PlayUISoundClip(audioClipKey, this.gameObject);
        }

     
	}
}
