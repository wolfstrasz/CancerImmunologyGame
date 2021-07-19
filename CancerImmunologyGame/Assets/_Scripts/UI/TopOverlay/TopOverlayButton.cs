using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace ImmunotherapyGame.UI.TopOverlay
{
    [RequireComponent (typeof(Selectable))]
    public class TopOverlayButton : UIMenuNode
    {
        [Header("Overlay Button")]
        [SerializeField]
        private TopOverlayButtonData buttonData = null;
        [SerializeField]
        private Animator anim = null;
        [SerializeField]
        private Image imageRender = null;
        [SerializeField]
        private Image glowImageRender = null;


        void Awake()
		{
            buttonData.onChangedStatus += Activate;
            Debug.Log("ButtonData " + buttonData.name + " is unlocked = " + buttonData.unlocked);
            gameObject.SetActive(buttonData.unlocked);
            imageRender.sprite = buttonData.icon;
            glowImageRender.material = buttonData.glowMaterial;
		}


        public void Animate(bool shouldAnimate)
		{
            if (shouldAnimate)
			{
                gameObject.SetActive(true);
                anim.SetTrigger("Pinged");
            }
            else
			{
                anim.SetTrigger("Opened");
            }

		}

        public void Activate(bool shouldActivate)
		{
            Debug.Log("Activate:  " + buttonData.name + " is unlocked = " + buttonData.unlocked + " should Activate = " + shouldActivate);

            gameObject.SetActive(shouldActivate);
		}


        public override void OnPointerEnter(PointerEventData eventData)
		{
            Debug.Log("UMN: POINTER_ENTER -> " + gameObject.name);
            OnSelect(eventData);
        }

		protected override void OnDisable()
		{
			base.OnDisable();
            buttonData.onChangedAnimationStatus -= Animate;
		}

		protected override void OnEnable()
		{
			buttonData.onChangedAnimationStatus += Animate;
		}

		void OnDestroy()
		{
            buttonData.onChangedStatus -= Activate;
		}

		protected override void OnPointerLeftClick(PointerEventData eventData)
		{
			anim.SetTrigger("Opened");
            if (buttonData.onOpenMenu != null)
            {
                buttonData.onOpenMenu();
            }
        }

		public override void OnPointerExit(PointerEventData eventData)
		{
            Debug.Log("UMN: POINTER_EXIT -> " + gameObject.name);
            OnDeselect(eventData);
		}
	}
}
