using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace ImmunotherapyGame.UI.InGameUI
{
    [RequireComponent (typeof(Selectable))]
    public class InGameUIButton : UIMenuNode, IPointerClickHandler
    {
        [SerializeField]
        private Image imageRender;
        [SerializeField]
        private Image glowImageRender;
        [SerializeField]
        private Animator anim;
        [SerializeField]
        private InGameUIButtonData buttonData;

        public void Animate()
		{
            gameObject.SetActive(true);
            anim.SetTrigger("Pinged");
		}

        internal void ApplyData(InGameUIButtonData data)
		{
            this.buttonData = data;
            gameObject.SetActive(data.unlocked);
            imageRender.sprite = data.icon;
            glowImageRender.material = data.glowMaterial;
		}

        private void OpenMenu()
		{
            anim.SetTrigger("Opened");
            buttonData.OpenMenu();
        }

        public void OpenMenu(InputAction.CallbackContext context)
		{
            Debug.Log(gameObject.name + ": requested Open menu from Input");
            OpenMenu();
		}

		public void OnPointerClick(PointerEventData eventData)
		    => OpenMenu();

        public override void OnPointerEnter(PointerEventData eventData)
           => OnSelect(eventData);
    }
}
