using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.UI
{
    [CreateAssetMenu]
    public class InGameUIButtonData : ScriptableObject
    {
        public List<InGameUIButtonData> connectedButtons;
        public bool unlocked;
        public Sprite icon;
        public Material glowMaterial;

        public delegate void OnChangedStatus();
        public OnChangedStatus onChangedStatus;

        public delegate void OnOpenMenu();
        public OnOpenMenu onOpenMenu;

        public delegate void OnCloseMenu();
        public OnCloseMenu onCloseMenu;

        public void PingChangedStatus()
		{
            if (onChangedStatus != null)
                onChangedStatus();
		}

        public void OpenMenu()
		{
            // Close other menus of the connected buttons
            foreach (var button in connectedButtons)
            {
                if (button.onCloseMenu != null)
                {
                    button.onCloseMenu();
                }
            }

            if (onOpenMenu != null)
            {
                onOpenMenu();
            }
        }
    }
}
