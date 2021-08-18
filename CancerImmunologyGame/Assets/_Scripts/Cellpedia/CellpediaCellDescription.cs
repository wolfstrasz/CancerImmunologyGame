using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.CellpediaSystem
{
    [CreateAssetMenu (menuName = "MyAssets/Cellpedia Cell Description")]
	[System.Serializable]
    public class CellpediaCellDescription : ScriptableObject, ISerializationCallbackReceiver
    {
		public string cellName;
		public Sprite sprite;
		[TextArea(5, 10)] public string description;
		[Range(0.8f, 2.0f)] public float spriteUIScale;

		[SerializeField] private Sprite noteSprite1;
		[SerializeField] private Sprite noteSprite2;
		[SerializeField] private Sprite noteSprite3;
		public string animatorTrigger;
		public CellDescriptionUnlockStatus status;

		public Vector3 spriteUIScaleVector => new Vector3(spriteUIScale, spriteUIScale, 1.0f);

		public Sprite Note1 => status.isSprite1Unlocked ? noteSprite1 : null;
		public Sprite Note2 => status.isSprite2Unlocked ? noteSprite2 : null;
		public Sprite Note3 => status.isSprite3Unlocked ? noteSprite3 : null;

		public bool IsUnlocked { get { return status.isUnlocked; } set { status.isUnlocked = value; } }
		public bool Note1IsUnlocked { get { return status.isSprite1Unlocked; } set { status.isSprite1Unlocked = value; } }
		public bool Note2IsUnlocked { get { return status.isSprite2Unlocked; } set { status.isSprite2Unlocked = value; } }
		public bool Note3IsUnlocked { get { return status.isSprite3Unlocked; } set { status.isSprite3Unlocked = value; } }

		public void OnAfterDeserialize()
		{
			status.isUnlocked = false;
			status.isSprite1Unlocked = false;
			status.isSprite2Unlocked = false;
			status.isSprite3Unlocked = false;
		}

		public void OnBeforeSerialize() {}

		internal void Reset()
		{
			status.isUnlocked = false;
			status.isSprite1Unlocked = false;
			status.isSprite2Unlocked = false;
			status.isSprite3Unlocked = false;
		}
	}

	[System.Serializable]
	public struct CellDescriptionUnlockStatus
	{
		public bool isUnlocked;
		public bool isSprite1Unlocked;
		public bool isSprite2Unlocked;
		public bool isSprite3Unlocked;
	}
}
