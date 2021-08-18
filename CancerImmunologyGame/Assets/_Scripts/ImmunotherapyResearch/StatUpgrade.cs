using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.Core;

namespace ImmunotherapyGame.ImmunotherapyResearchSystem
{
    [CreateAssetMenu (menuName = "MyAssets/Immunotherapy Stat Upgrade")]
    public class StatUpgrade : ScriptableObject, ISerializationCallbackReceiver
    {

        [Header("Description)")]
        public string title;
        public string description;
        public Sprite statThumbnailSprite;
        public Sprite generalThumbnailSprite;

        [Header("Status")]
        [SerializeField] private bool initialUnlockValue;
        [SerializeField] private float initialCurrentValue = 0;
        [SerializeField] [ReadOnly] internal bool unlocked; // Data to save

        [Header("Upgrades")]
        [SerializeField] [Expandable] internal StatAttribute statAttribute;
        [SerializeField] private List<UpgradeValues> upgrades = new List<UpgradeValues>();
        [SerializeField] internal int nextUpgradeIndex = 0; // Data to save

        [Header("Functional Values")]
        [SerializeField] [ReadOnly] private float currentValue;
        [SerializeField] [ReadOnly] private float appliedValue;

        internal bool HasAvailableUpgrade => nextUpgradeIndex < upgrades.Count ? true : false;
        internal float NextUpgradeValueChange => upgrades[nextUpgradeIndex].valueChange;
        internal int NextUpgradeCost => upgrades[nextUpgradeIndex].cost;
        internal float CurrentValue => currentValue;


        internal void ApplyEffect()
		{
            appliedValue = currentValue;
            statAttribute.CurrentValue += appliedValue; 
		}

        internal void RemoveEffect()
		{
            statAttribute.CurrentValue -= appliedValue;
            appliedValue = 0f;
		}


        internal int ApplyUpgradeAndReturnCost()
		{
            //statAttribute.CurrentValue = statAttribute.CurrentValue + upgrades[nextUpgradeIndex].valueChange;
            currentValue += upgrades[nextUpgradeIndex].valueChange;
            ++nextUpgradeIndex;
            return upgrades[nextUpgradeIndex - 1].cost;
		}


        internal void ApplyUpgradesFromStartToNextUpgradeIndex()
		{
            currentValue = initialCurrentValue;

            for (int j = 0; j < nextUpgradeIndex; ++j)
			{
                currentValue += upgrades[j].valueChange;
			}

            //statAttribute.CurrentValue = statAttribute.CurrentValue + fullValue;
        } 

        internal int ClearUpgradeAndReturnCost()
		{
            int overallCost = 0;
            float overallChange = 0f;

            while (--nextUpgradeIndex >= 0)
			{
                overallChange += upgrades[nextUpgradeIndex].valueChange;
                overallCost += upgrades[nextUpgradeIndex].cost;
			}

            //statAttribute.CurrentValue = statAttribute.CurrentValue - overallChange;
            currentValue -= overallChange;
            nextUpgradeIndex = 0;
            return overallCost;
		}

        public void Reset()
		{
            ClearUpgradeAndReturnCost();
            unlocked = initialUnlockValue;
		}

		public void OnBeforeSerialize() { }

		public void OnAfterDeserialize()
		{
            currentValue = initialCurrentValue;
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
