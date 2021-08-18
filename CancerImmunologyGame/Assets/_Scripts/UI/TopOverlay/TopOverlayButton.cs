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
        [SerializeField] private TopOverlayButtonData buttonData = null;
        [SerializeField] private GameObject buttonVisual = null;
        [SerializeField] private Animator anim = null;
        [SerializeField] private Image imageRender = null;
        [SerializeField] private Image glowImageRender = null;

        protected override void OnEnable()
        {
            imageRender.sprite = buttonData.icon;
            glowImageRender.material = buttonData.glowMaterial;
            buttonData.onChangedStatus += Activate;
            buttonData.onChangedAnimationStatus += Animate;
            buttonVisual.SetActive(buttonData.unlocked);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            buttonData.onChangedStatus -= Activate;
            buttonData.onChangedAnimationStatus -= Animate;
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
            buttonVisual.SetActive(shouldActivate);
		}

        public override void OnPointerEnter(PointerEventData eventData)
		{
            OnSelect(eventData);
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
            //Debug.Log("UMN: POINTER_EXIT -> " + gameObject.name);
            OnDeselect(eventData);
		}
	}
}
