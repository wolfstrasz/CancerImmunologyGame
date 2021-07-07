using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.Core;

namespace ImmunotherapyGame.ResearchAdvancement
{
    [CreateAssetMenu (menuName = "Data/StatUpgrade")]
    public class StatUpgrade : ScriptableObject
    {
        [Header("Description)")]
        [SerializeField] private bool initialUnlockValue;
        public bool unlocked; // Data to save
        public string title;
        public string description;
        public Sprite statThumbnailSprite;
        public Sprite generalThumbnailSprite;

        [Header("Functional")]
        [Expandable]
        public StatAttribute statAttribute;
        public int nextUpgradeIndex = 0; // Data to save
        public List<UpgradeValues> upgrades = new List<UpgradeValues>();

        public bool HasAvailableUpgrade => nextUpgradeIndex < upgrades.Count ? true : false;
        public int NextUpgradeCost => upgrades[nextUpgradeIndex].cost;
        public float NextUpgradeValueChange => upgrades[nextUpgradeIndex].valueChange;

        public int ApplyUpgradeAndReturnCost()
		{

            statAttribute.CurrentValue = statAttribute.CurrentValue + upgrades[nextUpgradeIndex].valueChange;
            ++nextUpgradeIndex;
            return upgrades[nextUpgradeIndex - 1].cost;

		}


        public void ApplyUpgradesFromStartToNextUpgradeIndex()
		{
            float fullValue = 0f;

            for (int j = 0; j < nextUpgradeIndex; ++j)
			{
                fullValue += upgrades[j].valueChange;
			}

            statAttribute.CurrentValue = statAttribute.CurrentValue + fullValue;
        } 

        public int ClearUpgradeAndReturnCost()
		{
            float overallChange = 0f;
            int overallCost = 0;
            for (int i = 0; i <  nextUpgradeIndex; ++i)
			{
                overallChange += upgrades[i].valueChange;
                overallCost += upgrades[i].cost;
			}

            statAttribute.CurrentValue = statAttribute.CurrentValue - overallChange;
            nextUpgradeIndex = 0;
            return overallCost;
		}

        public void Reset()
		{
            ClearUpgradeAndReturnCost();
            unlocked = initialUnlockValue;
		}

        [System.Serializable]
        public class UpgradeValues
        {
            public int cost;
            public float valueChange;
        }
    }

}
