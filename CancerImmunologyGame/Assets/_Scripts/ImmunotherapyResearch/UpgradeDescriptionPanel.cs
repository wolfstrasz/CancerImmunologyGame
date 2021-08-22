using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace ImmunotherapyGame.ImmunotherapyResearchSystem
{
    public class UpgradeDescriptionPanel : MonoBehaviour
    {
        [SerializeField] private StatUpgrade upgrade;
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text description;
        [SerializeField] private Image generalThumbnail;
        [SerializeField] private Image attributeThumbnail;
        [SerializeField] private TMP_Text upgradeValueText;
        [SerializeField] private TMP_Text currentValueText;

        internal void UpdateDisplay()
		{
            upgrade = ImmunotherapyResearch.Instance.CurrentStatUpgrade;
            if (upgrade == null)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);

            title.text = upgrade.title;
            description.text = upgrade.description;
            generalThumbnail.sprite = upgrade.generalThumbnailSprite;
            attributeThumbnail.sprite = upgrade.statThumbnailSprite;

            var currentValue = upgrade.CurrentValue;
            var attributeName = upgrade.statAttribute.attributeName;

            currentValueText.text = "Current: " + currentValue.ToString() + " " + attributeName;

            if (upgrade.HasAvailableUpgrade)
            {
                float valueIncrease = upgrade.NextUpgradeValueChange;
                string sign = upgrade.NextUpgradeValueChange > 0 ? "+" : "-";
                string text = "Next Upgrade: " + (valueIncrease + upgrade.CurrentValue).ToString()
                            + " (" + sign + Mathf.Abs(valueIncrease).ToString()
                            + ") " +  upgrade.statAttribute.attributeName;
                upgradeValueText.text = text;
			} 
            else
			{
                upgradeValueText.text = "Max Upgrade Reached";
			}
		}

    }
}
