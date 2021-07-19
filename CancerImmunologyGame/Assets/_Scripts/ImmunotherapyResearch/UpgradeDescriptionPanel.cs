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

            if (upgrade.HasAvailableUpgrade)
            {
                float valueIncrease = upgrade.NextUpgradeValueChange;
                string sign = upgrade.NextUpgradeValueChange > 0 ? "+" : "-";
                string text = "Next Upgrade: " + Mathf.Abs(valueIncrease).ToString()
                            +" (" + sign + Mathf.Abs(valueIncrease + upgrade.CurrentValue).ToString()
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
