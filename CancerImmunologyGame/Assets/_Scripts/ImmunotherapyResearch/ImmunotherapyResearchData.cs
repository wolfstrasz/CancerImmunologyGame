using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using ImmunotherapyGame.SaveSystem;


namespace ImmunotherapyGame.ImmunotherapyResearchSystem
{
	[CreateAssetMenu (menuName = "Data/Research Advancement Data")]
    public class ImmunotherapyResearchData : ScriptableObject
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
	public class SerializableImmunotherapyResearchData : SaveableObject
	{
		public int points = 0;
		public List<bool> isUnlocked = new List<bool>();
		public List<int> nextUpgradeIndex = new List<int>();

		public SerializableImmunotherapyResearchData(ImmunotherapyResearchData data)
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

		public SerializableImmunotherapyResearchData()
		{
			points = 0;
			isUnlocked = new List<bool>();
			nextUpgradeIndex = new List<int>();

	}

	public void CopyTo(ImmunotherapyResearchData data)
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
