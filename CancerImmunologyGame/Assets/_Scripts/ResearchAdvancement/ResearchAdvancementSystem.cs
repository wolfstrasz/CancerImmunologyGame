using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using ImmunotherapyGame.Core;
using ImmunotherapyGame.Core.SystemInterfaces;
using ImmunotherapyGame.SaveSystem;
using ImmunotherapyGame.UI.TopOverlay;
using ImmunotherapyGame.UI;


namespace ImmunotherapyGame.ResearchAdvancement
{
    public class ResearchAdvancementSystem : Singleton<ResearchAdvancementSystem>, IDataManager
    {
		[Header("Data")]
		[SerializeField]
		internal ResearchAdvancementData data = null;
		private SerializableResearchAdvancementData savedData = null;

		[Header("System UI")]
		[SerializeField]
		private InterfaceControlPanel researchAdvancementPanel = null;
		[SerializeField]
		private UpgradeDescriptionPanel upgradeDescriptionPanel = null;
		[SerializeField]
		private UpgradePurchasePanel upgradePurchasePanel = null;
		[SerializeField]
		internal GameObject currentSelectedStatUpgradeButton = null;


		[Header("Game UI")]
		[SerializeField]
		private TopOverlayButtonData inGameUIButtonData = null;

		// Input handling
		PlayerControls playerControls = null;
		InputAction openReseachAdvancementAction = null;
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
			//openReseachAdvancementAction = playerControls.Systems.Microscope;
		}

		public void Initialise()
		{
			// 1) Initialise all required components
			// 2) Set if button is visible 
			// 3) Add nodes to listen to#
			// 4) Set initialiser node
		}

		private void OnEnable()
		{
			inGameUIButtonData.onOpenMenu += OpenView;
			openReseachAdvancementAction.started += OpenView;
			researchAdvancementPanel.onOpenInterface += OnOpenView;
			researchAdvancementPanel.onCloseInterface += OnCloseView;
		}

		private void OnDisable()
		{
			inGameUIButtonData.onOpenMenu -= OpenView;
			openReseachAdvancementAction.started -= OpenView;
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

		}

		public void OnCloseView()
		{

		}

		// Start is called before the first frame update
		void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }


		public void LoadData()
		{
			throw new System.NotImplementedException();
		}


		public void SaveData()
		{
			savedData = new SerializableResearchAdvancementData(data);
			SaveManager.Instance.SaveData<SerializableResearchAdvancementData>(savedData);
		}

		public void ResetData()
		{
			data.Reset();
			inGameUIButtonData.unlocked = false;
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


	}
}
