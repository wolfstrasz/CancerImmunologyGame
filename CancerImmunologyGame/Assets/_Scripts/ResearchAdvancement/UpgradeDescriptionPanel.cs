using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace ImmunotherapyGame.ResearchAdvancement
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
            upgrade = ResearchAdvancementSystem.Instance.CurrentStatUpgrade;
            if (upgrade != null) return;

            title.text = upgrade.title;
            description.text = upgrade.description;
            generalThumbnail.sprite = upgrade.generalThumbnailSprite;
            attributeThumbnail.sprite = upgrade.statThumbnailSprite;

            bool hasUpgrade = upgrade.HasAvailableUpgrade;

            if (hasUpgrade)
            {
                string text = "";
                text = "Next Upgrade: " + upgrade.NextUpgradeValueChange + " " + upgrade.statAttribute.attributeName;
                upgradeValueText.text = text;
			} else
			{
                upgradeValueText.text = "Max Upgrade Reached";
			}
		}

    }
}
