using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.SaveSystem;

namespace ImmunotherapyGame.CellpediaSystem
{
	[CreateAssetMenu(menuName = "MyAssets/Cellpedia Data")]
	public class CellpediaData : ScriptableObject
	{
		public bool isSystemUnlocked;
		public List<CellpediaCellDescription> cellpediaItems;

		internal void ResetData()
		{
			if (cellpediaItems.Count <= 0)
			{
				Debug.LogWarning("Level list is empty! Not possible to reset it!");
				return;
			}

			for (int i = 0; i < cellpediaItems.Count; ++i)
			{
				cellpediaItems[i].Reset();
			}
			isSystemUnlocked = false;
		}
	}

	[System.Serializable]
	public class SerializableCellpediaData : SaveableObject
	{
		public bool isSystemUnlocked = false;

		public int count = 0;
		public List<CellDescriptionUnlockStatus> unlockStatus = new List<CellDescriptionUnlockStatus>();

		public SerializableCellpediaData(CellpediaData data)
		{
			count = data.cellpediaItems.Count;
			unlockStatus = new List<CellDescriptionUnlockStatus>();
			for (int i = 0; i < count; ++i)
			{
				unlockStatus.Add(data.cellpediaItems[i].status);
			}
			isSystemUnlocked = data.isSystemUnlocked;
		}

		public SerializableCellpediaData()
		{
			count = 0;
			unlockStatus = new List<CellDescriptionUnlockStatus>();
			isSystemUnlocked = false;
		}

		public void CopyTo(CellpediaData data)
		{
			data.isSystemUnlocked = isSystemUnlocked;
			for (int i = 0; i < count; ++i)
			{
				data.cellpediaItems[i].status = unlockStatus[i];
			}
		}
	}

}