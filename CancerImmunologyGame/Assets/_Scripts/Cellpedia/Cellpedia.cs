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
		[SerializeField] internal CellpediaData data = null;
		private SerializableCellpediaData savedData = null;
		[ReadOnly] private CellpediaCellDescription selectedCellDescription = null;

		[Header("Cellpedia UI")]
		[SerializeField] private InterfaceControlPanel cellpediaPanel = null;
		[SerializeField] internal CellpediaNotepad notepad = null;
		[SerializeField] internal CellpediaMicroscope microscope = null;
		[SerializeField] internal CellpediaButtonsBar buttonsBar = null;

		[Header ("Popups")]
		[SerializeField] private GameObject popupLayout = null;
		[SerializeField] private GameObject popupPrefab = null;

		[Header("Game UI")]
		[SerializeField] private TopOverlayButtonData inGameUIButtonData = null;
		[SerializeField] [ReadOnly] private bool isFeatureUnlocked = false;

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
			isFeatureUnlocked = data.isSystemUnlocked;

			selectedCellDescription = null;
			buttonsBar.Initialise();
			microscope.Initialise();
		
			inGameUIButtonData.PingUnlockStatus(isFeatureUnlocked);

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
			if (!isFeatureUnlocked) return; // TODO: remove hidden state

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
				selectedCellDescription = PetridishButton.lastSubmitted.cellDescription;
			}
			else // Find the first available one
			{
				for (int i = 0; i < data.cellpediaItems.Count; ++i)
				{
					if (data.cellpediaItems[i].IsUnlocked)
					{
						selectedCellDescription = data.cellpediaItems[i];
						break;
					}
				}
			}

			if (selectedCellDescription == null) 
			{
				buttonsBar.gameObject.SetActive(false);
				microscope.gameObject.SetActive(false);
				notepad.gameObject.SetActive(false);
			} 
			else
			{
				buttonsBar.gameObject.SetActive(true);
				microscope.gameObject.SetActive(true);
				notepad.gameObject.SetActive(true);
				buttonsBar.OnOpen(selectedCellDescription);
				microscope.OnOpen(selectedCellDescription);
				notepad.OnOpen(selectedCellDescription);
			}

			cellpediaPanel.initialControlNode = PetridishButton.lastSubmitted;
		}

		private void OnCloseView()
		{
			microscope.OnClose();
		}

		public void UnlockFeature()
		{
			isFeatureUnlocked = true;
			data.isSystemUnlocked = true;
			inGameUIButtonData.PingUnlockStatus(isFeatureUnlocked);
		}

		// Cell description handling
		public void UnlockCellDescription(CellpediaCellDescription cellDescription)
		{
			cellDescription.IsUnlocked = true;
			buttonsBar.ActivateButton(cellDescription);

			CellpediaPopup popup = Instantiate(popupPrefab, popupLayout.transform, false).GetComponent<CellpediaPopup>();
			popup.SetInfo(cellDescription);

			inGameUIButtonData.PingAnimationStatus(true);
		}


		// Data handling
		public void LoadData()
		{
			savedData = SaveManager.Instance.LoadData<SerializableCellpediaData>();

			if (savedData == null)
			{
				Debug.Log("No previous saved data found. Creating new level data save.");
				ResetData();
				SaveData();
			}
			else
			{
				savedData.CopyTo(data);
			}

			isFeatureUnlocked = data.isSystemUnlocked;
			inGameUIButtonData.PingUnlockStatus(isFeatureUnlocked);
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