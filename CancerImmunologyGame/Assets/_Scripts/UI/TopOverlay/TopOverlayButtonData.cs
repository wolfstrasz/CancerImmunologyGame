using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.UI.TopOverlay
{
    [CreateAssetMenu]
    public class TopOverlayButtonData : ScriptableObject
    {
        public bool unlocked;
        public Sprite icon;
        public Material glowMaterial;

        public delegate void OnChangedStatus();
        public OnChangedStatus onChangedStatus;

        public delegate void OnOpenMenu();
        public OnOpenMenu onOpenMenu;

        public void PingChangedStatus()
		{
            if (onChangedStatus != null)
                onChangedStatus();
		}

    }
}
