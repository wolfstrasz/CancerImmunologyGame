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
        public List<StatUpgrade> statUpgrades;

		internal void Reset()
		{
			points = 0;
			for (int i = 0; i < statUpgrades.Count; ++i)
			{
				statUpgrades[i].Reset();
			}
		}
    }

	[System.Serializable]
	public class SerializableResearchAdvancementData : SaveableObject
	{
		public int points = 0;
		public List<bool> isUnlocked = new List<bool>();
		public List<int> nextUpgradeIndex = new List<int>();

		public SerializableResearchAdvancementData(ResearchAdvancementData data)
		{

			points = data.points;
			isUnlocked = new List<bool>();
			nextUpgradeIndex = new List<int>();

			for (int i = 0; i < data.statUpgrades.Count; ++i)
			{
				isUnlocked.Add(data.statUpgrades[i].unlocked);
				nextUpgradeIndex.Add(data.statUpgrades[i].nextUpgradeIndex);
			}
		}

		public SerializableResearchAdvancementData()
		{
			points = 0;
			isUnlocked = new List<bool>();
			nextUpgradeIndex = new List<int>();

	}

	public void CopyTo(ResearchAdvancementData data)
	{
		data.points = points;
		for (int i = 0; i < isUnlocked.Count; ++i)
		{
			data.statUpgrades[i].unlocked = isUnlocked[i];
			data.statUpgrades[i].nextUpgradeIndex = nextUpgradeIndex[i];
		}
	}
	}
}
