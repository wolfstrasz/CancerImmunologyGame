using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.Tutorials
{
    [CreateAssetMenu (menuName = "MyAssets/Tutorial Log")]
    public class TutorialLog : ScriptableObject, ISerializationCallbackReceiver
    {
        public bool isUnlocked;
		public string title;
        public Sprite imageSprite;

		public void OnAfterDeserialize()
		{
			isUnlocked = false;
		}

		public void OnBeforeSerialize()
		{
		}
	}
}
