using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.SaveSystem;

namespace ImmunotherapyGame.CellpediaSystem
{
	[CreateAssetMenu(menuName = "Cellpedia Data")]
	public class CellpediaData : ScriptableObject
	{
		public List<CellpediaObject> cellpediaItems;

		internal void ResetData()
		{
			if (cellpediaItems.Count <= 0)
			{
				Debug.LogError("Level list is empty! Not possible to reset it!");
				return;
			}

			for (int i = 0; i < cellpediaItems.Count; ++i)
			{
				cellpediaItems[i].isUnlocked = false;
			}
		}
	}

	[System.Serializable]
	public class CellpediaObject
	{
		public CellpediaItemTypes type = CellpediaItemTypes.NONE;
		public string cellname;
		[TextArea(5, 10)] public string description;

		public Sprite sprite;
		[Range(0.8f, 2.0f)]
		public float spriteUIScale;
		public Vector3 spriteUIScaleVector => new Vector3(spriteUIScale, spriteUIScale, 1.0f);

		public Sprite noteSprite1;
		public Sprite noteSprite2;
		public Sprite noteSprite3;

		public string animatorTrigger;

		public bool isUnlocked = false;
	}

	[System.Serializable]
	public class SerializableCellpediaData : SavableObject
	{
		public int count = 0;
		public List<bool> isUnlocked = new List<bool>();

		public SerializableCellpediaData(CellpediaData data)
		{
			count = data.cellpediaItems.Count;
			isUnlocked = new List<bool>();
			for (int i = 0; i < count; ++i)
			{
				isUnlocked.Add(data.cellpediaItems[i].isUnlocked);
			}
		}

		public SerializableCellpediaData()
		{
			count = 0;
			isUnlocked = new List<bool>();
		}

		public void CopyTo(CellpediaData data)
		{
			for (int i = 0; i < count; ++i)
			{
				data.cellpediaItems[i].isUnlocked = isUnlocked[i];
			}
		}
	}

	public enum CellpediaItemTypes { NONE, TKILLER, THELPER, DENDRITIC, REGULATORY, CANCER, CAF }

}