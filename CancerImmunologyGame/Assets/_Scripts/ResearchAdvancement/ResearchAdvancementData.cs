using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using ImmunotherapyGame.SaveSystem;


namespace ImmunotherapyGame.ResearchAdvancement
{
	[CreateAssetMenu (menuName = "Data/Research Advancement Data")]
    public class ResearchAdvancementData : ScriptableObject
    {
		public int points;
        public List<StatUpgrade> dataObjects;

		internal void Reset()
		{

		}
    }

	[System.Serializable]
	public class SerializableResearchAdvancementData : SavableObject
	{
		public int points = 0;
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
