using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using ImmunotherapyGame.Audio;

namespace ImmunotherapyGame.UI
{
    [RequireComponent(typeof(Slider))]
    public class MenuSlider : UIMenuNode
    {
        [SerializeField] private float sliderSoundTimeout = 0.1f;
        [SerializeField][ReadOnly] private float timeout = 0f;

        void Update()
		{
            if (timeout > 0f)
			{
                timeout -= Time.deltaTime;
                if (timeout < 0f)
                {
                    AudioManager.Instance.PlayUISoundClip(audioClipKey, this.gameObject);
                }
            }
		}
		// Cached objects
		private Slider slider = null;

        // Private methods
        private void OnValueChanged()
        {
            timeout = sliderSoundTimeout;
        }


		// Protected methods
		void Awake() 
            => slider = GetComponent<Slider>();

        override protected void OnEnable()
		{
            base.OnEnable();
            slider.onValueChanged.AddListener(delegate { OnValueChanged(); });
		}


        protected override void OnDisable()
		{
            base.OnDisable();
            slider.onValueChanged.RemoveListener(delegate { OnValueChanged(); });
        }

	}
}
