using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using ImmunotherapyGame.Core;
using ImmunotherapyGame.Core.SystemInterfaces;
using ImmunotherapyGame.SaveSystem;
using ImmunotherapyGame.UI.TopOverlay;
using ImmunotherapyGame.UI;

namespace ImmunotherapyGame.CellpediaSystem
{
	public class Cellpedia : Singleton<Cellpedia>, IDataManager
	{
		[Header("Data")]
		[SerializeField]
		internal CellpediaData data = null;
		private SerializableCellpediaData savedData = null;
		[ReadOnly]
		private CellpediaObject selectedCellObject = null;

		[Header("Cellpedia UI")]
		[SerializeField]
		private InterfaceControlPanel cellpediaPanel = null;
		[SerializeField]
		internal CellpediaNotepad notepad = null;
		[SerializeField]
		internal CellpediaMicroscope microscope = null;
		[SerializeField]
		internal CellpediaButtonsBar buttonsBar = null;

		[Header ("Popups")]
		[SerializeField]
		private GameObject popupLayout = null;
		[SerializeField]
		private GameObject popupPrefab = null;

		[Header("Game UI")]
		[SerializeField]
		private TopOverlayButtonData inGameUIButtonData = null;
		[SerializeField] [ReadOnly] private bool unlockedFeature = false;

		// Input handling
		private PlayerControls playerControls = null;
		// Used by tutorials
		public bool IsCellpediaOpened => cellpediaPanel.IsOpened;


		protected override void Awake()
		{
			base.Awake();
			playerControls = new PlayerControls();
			playerControls.Enable();
		}

		public void Initialise()
		{
			unlockedFeature = false;
			selectedCellObject = null;
			buttonsBar.Initialise();
			microscope.Initialise();

			// Set if button is visible
			for	(int i = 0; i < data.cellpediaItems.Count; ++i)
			{
				unlockedFeature |= data.cellpediaItems[i].isUnlocked;
			}

			inGameUIButtonData.PingUnlockStatus(unlockedFeature);

			// Add nodes to listen to
			for (int i = 0; i < buttonsBar.petridishButtonList.Count; ++i)
			{
				cellpediaPanel.nodesToListen.Add(buttonsBar.petridishButtonList[i]);
			}
		}

		private void OnEnable()
		{
			inGameUIButtonData.onOpenMenu += OpenView;
			playerControls.Systems.CellpediaMenu.started += OpenView;
			cellpediaPanel.onOpenInterface += OnOpenView;
			cellpediaPanel.onCloseInterface += OnCloseView;
		}

		private void OnDisable()
		{
			inGameUIButtonData.onOpenMenu -= OpenView;
			playerControls.Systems.CellpediaMenu.started -= OpenView;
			cellpediaPanel.onOpenInterface -= OnOpenView;
			cellpediaPanel.onCloseInterface -= OnCloseView;
		}

		// UI  Buttons callback
		public void OpenView()
		{
			cellpediaPanel.Open();
		}

		public void CloseView()
		{
			cellpediaPanel.Close();
		}

		// Input callback
		public void OpenView(InputAction.CallbackContext context)
		{
			Debug.Log("Button Call for Cellpedia");
			if (!unlockedFeature) return; // TODO: remove hidden state

			if (cellpediaPanel.IsOpened)
			{
				Debug.Log("Try to close Cellpedia");

				cellpediaPanel.Close();
			}
			else
			{
				Debug.Log("Try to open Cellpedia");

				cellpediaPanel.Open();
			}
		}

		private void OnOpenView()
		{
			inGameUIButtonData.PingAnimationStatus(false);

			if (PetridishButton.lastSubmitted != null)
			{
				selectedCellObject = PetridishButton.lastSubmitted.cellObject;
			}
			else // Find the first available one
			{
				for (int i = 0; i < data.cellpediaItems.Count; ++i)
				{
					if (data.cellpediaItems[i].isUnlocked)
					{
						selectedCellObject = data.cellpediaItems[i];
						break;
					}
				}
			}

			buttonsBar.OnOpen(selectedCellObject);
			microscope.OnOpen(selectedCellObject);
			notepad.OnOpen(selectedCellObject);

			cellpediaPanel.initialControlNode = PetridishButton.lastSubmitted;
		}

		private void OnCloseView()
		{
			microscope.OnClose();
		}

		// Cell description handling
		public void UnlockCellDescription(CellpediaItemTypes type)
		{
			var items = data.cellpediaItems;
			CellpediaObject item = null;

			// Search for item
			for (int i = 0; i < items.Count; ++i)
			{
				if (items[i].type == type)
				{
					item = items[i];
					break;
				}
			}

			if (item.isUnlocked) return;


			item.isUnlocked = true;
			buttonsBar.ActivateButton(item);

			CellpediaPopup popup = Instantiate(popupPrefab, popupLayout.transform, false).GetComponent<CellpediaPopup>();
			popup.SetInfo(item);

			unlockedFeature = true;
			inGameUIButtonData.PingUnlockStatus(unlockedFeature);
			inGameUIButtonData.PingAnimationStatus(true);
		}

		// Data handling
		public void LoadData()
		{
			savedData = SaveManager.Instance.LoadData<SerializableCellpediaData>();

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
			savedData = new SerializableCellpediaData(data);
			SaveManager.Instance.SaveData<SerializableCellpediaData>(savedData);
		}

		public void ResetData()
		{
			data.ResetData();
			inGameUIButtonData.PingUnlockStatus(false);
		}
	}

}