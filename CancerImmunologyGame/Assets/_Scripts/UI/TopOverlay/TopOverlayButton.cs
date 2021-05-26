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
    public class TopOverlayButton : UIMenuNode, IPointerClickHandler
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


        public void Animate()
		{
            gameObject.SetActive(true);
            anim.SetTrigger("Pinged");
		}

		public void OnPointerClick(PointerEventData eventData)
		{
            anim.SetTrigger("Opened");
            if (buttonData.onOpenMenu != null)
			{
                buttonData.onOpenMenu();
			}
        }

        public override void OnPointerEnter(PointerEventData eventData)
           => OnSelect(eventData);


        void OnEnable()
		{
            buttonData.onChangedStatus += Animate;
		}

        void OnDestroy()
		{
            buttonData.onChangedStatus -= Animate;
		}
    }
}
