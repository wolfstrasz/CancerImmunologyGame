using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using ImmunotherapyGame.SaveSystem;


namespace ImmunotherapyGame.ResearchAdvancement
{
    public class ResearchAdvancementData : ScriptableObject
    {
        List<ResearchAdvancementObject> dataObjects;

		internal void Reset()
		{

		}
    }


    [System.Serializable]
    public class ResearchAdvancementObject
	{
		// Object that stores data
		// Maybe per upgrade
	}

	[System.Serializable]
	public class SerializableResearchAdvancementData : SavableObject
	{
		public int count = 0;
		public List<bool> isUnlocked = new List<bool>();

		public SerializableResearchAdvancementData(ResearchAdvancementData data)
		{
			//count = data.cellpediaItems.Count;
			//isUnlocked = new List<bool>();
			//for (int i = 0; i < count; ++i)
			//{
			//	isUnlocked.Add(data.cellpediaItems[i].isUnlocked);
			//}
		}

		public SerializableResearchAdvancementData()
		{
			//count = 0;
			//isUnlocked = new List<bool>();
		}

		public void CopyTo(ResearchAdvancementData data)
		{
			//for (int i = 0; i < count; ++i)
			//{
			//	data.cellpediaItems[i].isUnlocked = isUnlocked[i];
			//}
		}
	}
}
