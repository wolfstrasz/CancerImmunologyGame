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


        public override void OnPointerEnter(PointerEventData eventData)
           => OnSelect(eventData);


        protected override void OnEnable()
		{
            buttonData.onChangedStatus += Animate;
		}

        void OnDestroy()
		{
            buttonData.onChangedStatus -= Animate;
		}

		protected override void OnPointerLeftClick(PointerEventData eventData)
		{
			anim.SetTrigger("Opened");
            if (buttonData.onOpenMenu != null)
            {
                buttonData.onOpenMenu();
            }
        }
	}
}
