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

        public delegate void OnChangedStatus(bool status);
        public OnChangedStatus onChangedStatus;

        public delegate void OnChangedAnimationStatus(bool status);
        public OnChangedAnimationStatus onChangedAnimationStatus;

        public delegate void OnOpenMenu();
        public OnOpenMenu onOpenMenu;

        public void PingAnimationStatus(bool status)
		{
            if (onChangedAnimationStatus != null)
			{
                onChangedAnimationStatus(status);
			}
		}

        public void PingUnlockStatus(bool status)
		{
            unlocked = status;

            if (onChangedStatus != null)
			{
                onChangedStatus(status);
			}
		}

    }
}
