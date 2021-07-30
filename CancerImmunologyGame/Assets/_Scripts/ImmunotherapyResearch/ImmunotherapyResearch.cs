using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

using ImmunotherapyGame.Core;
using ImmunotherapyGame.Core.SystemInterfaces;
using ImmunotherapyGame.SaveSystem;
using ImmunotherapyGame.UI.TopOverlay;
using ImmunotherapyGame.UI;


namespace ImmunotherapyGame.ImmunotherapyResearchSystem
{
    public class ImmunotherapyResearch : Singleton<ImmunotherapyResearch>, IDataManager
    {
		[Header("Data")]
		[SerializeField]
		internal ImmunotherapyResearchData data = null;
		private SerializableImmunotherapyResearchData savedData = null;

		[Header("System UI")]
		[SerializeField]
		private InterfaceControlPanel researchAdvancementPanel = null;
		[SerializeField]
		private UpgradeDescriptionPanel upgradeDescriptionPanel = null;
		[SerializeField]
		private UpgradePurchasePanel upgradePurchasePanel = null;
		[SerializeField]
		internal GameObject currentSelectedStatUpgradeButton = null;
		[SerializeField]
		private List<StatUpgradeButton> allButtons = new List<StatUpgradeButton>();

		[Header("Game UI")]
		[SerializeField]
		private TopOverlayButtonData inGameUIButtonData = null;
		[SerializeField]
		private Animator machineAnimator = null;
		[SerializeField]
		private TMPro.TMP_Text currentPointsText = null;

		[Header("Reset Points Panel")]
		[SerializeField]
		private GameObject resetPointsPanel = null;
		[SerializeField]
		private GameObject resetPointsPanelCancelButton = null;

		// Input handling
		PlayerControls playerControls = null;
		private bool unlockedFeature = false;

		private StatUpgrade currentStatUpgrade = null;
		internal StatUpgrade CurrentStatUpgrade
		{
			get
			{
				return currentStatUpgrade;
			}
		}


		protected override void Awake()
		{
			base.Awake();
			playerControls = new PlayerControls();
			playerControls.Enable();
		}

		public void Initialise()
		{
			unlockedFeature = false;
			currentSelectedStatUpgradeButton = null;
			upgradeDescriptionPanel.UpdateDisplay();
			upgradePurchasePanel.UpdateDisplay();

			researchAdvancementPanel.nodesToListen.Add(researchAdvancementPanel.initialControlNode);
			Debug.Log("RES: " + " UnlockedFeature = " + unlockedFeature);

			// Apply data upgrades 
			for (int i = 0; i < data.statUpgrades.Count; ++i)
			{
				data.statUpgrades[i].ApplyUpgradesFromStartToNextUpgradeIndex();

				// 4) Set if GameUI button is visible 
				unlockedFeature |= data.statUpgrades[i].unlocked;
			}

			Debug.Log("RES: " + " UnlockedFeature = " + unlockedFeature);

			// 1) Initialise all required components
			for (int i = 0; i < allButtons.Count; ++i)
			{
				allButtons[i].Initialise();
				// 2) update all buttons
				allButtons[i].UpdateDisplay();

				// 3) Add nodes to listen to
				researchAdvancementPanel.nodesToListen.Add(allButtons[i]);
			}

			Debug.Log("RES: " + " UnlockedFeature = " + unlockedFeature);
			inGameUIButtonData.PingUnlockStatus(unlockedFeature);
			Debug.Log("RES: " + " UnlockedFeature = " + unlockedFeature);

			// 4) Set initialiser node
			//researchAdvancementPanel.initialControlNode = null;

		}

		private void OnEnable()
		{
			inGameUIButtonData.onOpenMenu += OpenView;
			playerControls.Systems.ImmunotherapyResearchMenu.started += OpenView;
			researchAdvancementPanel.onOpenInterface += OnOpenView;
			researchAdvancementPanel.onCloseInterface += OnCloseView;
		}

		private void OnDisable()
		{
			inGameUIButtonData.onOpenMenu -= OpenView;
			playerControls.Systems.ImmunotherapyResearchMenu.started -= OpenView;
			researchAdvancementPanel.onOpenInterface -= OnOpenView;
			researchAdvancementPanel.onCloseInterface -= OnCloseView;
		}

		// UI  Buttons callback
		public void OpenView()
		{
			researchAdvancementPanel.Open();
		}

		public void CloseView()
		{
			researchAdvancementPanel.Close();
		}

		// Input callback
		public void OpenView(InputAction.CallbackContext context)
		{
			if (!unlockedFeature) return; // TODO: remove hidden state

			if (researchAdvancementPanel.IsOpened)
			{
				researchAdvancementPanel.Close();
			}
			else
			{
				researchAdvancementPanel.Open();
			}
		}

		public void OnOpenView()
		{
			currentPointsText.text = data.points.ToString();

			inGameUIButtonData.PingAnimationStatus(false);

			for (int i = 0; i< allButtons.Count; ++i)
			{
				allButtons[i].UpdateDisplay();
			}
		}

		public void CloseResetPointsPanel()
		{
			resetPointsPanel.SetActive(false);
			EventSystem.current.SetSelectedGameObject(researchAdvancementPanel.initialControlNode.gameObject);
		}

		public void OpenResetPointsPanel()
		{
			resetPointsPanel.SetActive(true);
			EventSystem.current.SetSelectedGameObject(resetPointsPanelCancelButton);
		}

		public void ResetPoints()
		{
			int sum = 0;
			for (int i = 0; i < data.statUpgrades.Count; ++i)
			{
				sum += data.statUpgrades[i].ClearUpgradeAndReturnCost();
			}
			data.points += sum;
			currentPointsText.text = data.points.ToString();
			CloseResetPointsPanel();
			currentSelectedStatUpgradeButton = null;
			currentStatUpgrade = null;
			upgradeDescriptionPanel.UpdateDisplay();
			upgradePurchasePanel.UpdateDisplay();

		}

		internal void BuyCurrentSelectedUpgrade()
		{
			if (currentStatUpgrade == null)
			{
				Debug.LogWarning("Buy Current Selected Upgrade is called but the value is currently null");
				
			}
			else
			{
				machineAnimator.SetTrigger("UpgradeBuy");
				data.points -= currentStatUpgrade.ApplyUpgradeAndReturnCost();
				currentPointsText.text = data.points.ToString();

				if (!currentStatUpgrade.HasAvailableUpgrade || currentStatUpgrade.NextUpgradeCost > data.points )
				{
					EventSystem.current.SetSelectedGameObject(currentSelectedStatUpgradeButton);
				}

				upgradePurchasePanel.UpdateDisplay();
				upgradeDescriptionPanel.UpdateDisplay();
			}
		}

		public void UnlockUpgrades(List<StatUpgrade> upgradesToUnlock)
		{

			bool notNewUnlock = true;
			for (int i = 0; i < upgradesToUnlock.Count; ++i)
			{
				notNewUnlock &= upgradesToUnlock[i].unlocked;
				upgradesToUnlock[i].unlocked = true;
			}

			if (!notNewUnlock)
			{
				unlockedFeature = true;
				inGameUIButtonData.PingUnlockStatus(unlockedFeature);
				inGameUIButtonData.PingAnimationStatus(unlockedFeature);
			}

		}

		public void AddPoints(int pointsToAdd)
		{
			data.points += pointsToAdd;
			currentPointsText.text = data.points.ToString();
			upgradePurchasePanel.UpdateDisplay();
		}

		public void OnCloseView()
		{
			resetPointsPanel.SetActive(false);
		}



		internal void SelectStatUpgrade(StatUpgradeButton button)
		{
			// Cache data
			currentStatUpgrade = button.statUpgrade;
			currentSelectedStatUpgradeButton = button.gameObject;

			// Update panels
			upgradeDescriptionPanel.gameObject.SetActive(true);
			upgradeDescriptionPanel.UpdateDisplay();

			upgradePurchasePanel.UpdateDisplay();
		}

		internal void OnPurchaseButtonCancel()
		{
			EventSystem.current.SetSelectedGameObject(currentSelectedStatUpgradeButton);
			currentStatUpgrade = null;
			currentSelectedStatUpgradeButton = null;
			upgradeDescriptionPanel.UpdateDisplay();
			upgradePurchasePanel.UpdateDisplay();
		}


		// Data Handling
		public void LoadData()
		{
			savedData = SaveManager.Instance.LoadData<SerializableImmunotherapyResearchData>();

			if (savedData == null)
			{
				Debug.Log("No previous saved data found. Creating new level data save.");
			}
			else
			{
				savedData.CopyTo(data);
			}
			SaveData();
			
		}


		public void SaveData()
		{
			savedData = new SerializableImmunotherapyResearchData(data);
			SaveManager.Instance.SaveData<SerializableImmunotherapyResearchData>(savedData);
		}

		public void ResetData()
		{
			data.Reset();

			inGameUIButtonData.PingUnlockStatus(false);
		}


	}
}
