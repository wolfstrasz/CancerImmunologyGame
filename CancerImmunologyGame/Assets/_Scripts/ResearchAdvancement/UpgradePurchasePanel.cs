using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace ImmunotherapyGame.ResearchAdvancement
{
    public class UpgradePurchasePanel : MonoBehaviour
    {
        [Header ("Data")]
        [SerializeField] private ResearchAdvancementData data;
        [SerializeField] private StatUpgrade statUpgrade = null;

        [Header("UI Elements")]
        [SerializeField] private GameObject purchaseButton;
        [SerializeField] private GameObject notEnoughPointsObject;

        [SerializeField] private TMP_Text costText;
        [SerializeField] private TMP_Text notEnoughPointsText;

        internal void UpdateDisplay()
		{
            statUpgrade = ResearchAdvancementSystem.Instance.CurrentStatUpgrade;


            if (statUpgrade == null || !statUpgrade.HasAvailableUpgrade)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);

            if (data.points < statUpgrade.NextUpgradeCost)
			{
                purchaseButton.SetActive(false);
                notEnoughPointsObject.SetActive(true);
                notEnoughPointsText.text = data.points + " / " + statUpgrade.NextUpgradeCost;
                return;
			}

            notEnoughPointsObject.SetActive(false);
            purchaseButton.SetActive(true);

            costText.text = "Cost: " + statUpgrade.NextUpgradeCost;
            EventSystem.current.SetSelectedGameObject(purchaseButton);
		}
    }
}
