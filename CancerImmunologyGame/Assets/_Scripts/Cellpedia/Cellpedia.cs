using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.Core;
using ImmunotherapyGame.Core.SystemInterfaces;
using ImmunotherapyGame.SaveSystem;
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
		private GameObject cellpediaView = null;
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

		[Header ("Game UI")]
		[SerializeField]
		private GameObject microscopeButton = null;
		[SerializeField]
		private Animator microscopeButtonAnimator = null;

		// Used by Cellpedia popups
		internal Transform PopupLayout => popupLayout.transform;
		internal Vector3 MicroscopeButtonPosition => microscopeButton.transform.position;

		// Used by tutorials
		public bool IsCellpediaOpened => cellpediaView.gameObject.activeSelf;


		public void Update()
		{
			//if (Input.GetKeyDown(KeyCode.Q))
			//{
			//	UnlockCellDescription(CellpediaItemTypes.TKILLER);
			//}
			//if (Input.GetKeyDown(KeyCode.W))
			//{
			//	UnlockCellDescription(CellpediaItemTypes.DENDRITIC);
			//}
			//if (Input.GetKeyDown(KeyCode.E))
			//{
			//	UnlockCellDescription(CellpediaItemTypes.THELPER);
			//}
			//if (Input.GetKeyDown(KeyCode.R))
			//{
			//	UnlockCellDescription(CellpediaItemTypes.CANCER);
			//}
			//if (Input.GetKeyDown(KeyCode.T))
			//{
			//	UnlockCellDescription(CellpediaItemTypes.REGULATORY);
			//}
			//if (Input.GetKeyDown(KeyCode.Y))
			//{
			//	UnlockCellDescription(CellpediaItemTypes.CAF);
			//}
		}

		public void Initialise()
		{
			selectedCellObject = null;
			buttonsBar.Initialise();
			microscope.Initialise();
		}

		// UI Button callbacks
		public void CloseView()
		{
			microscope.OnClose();
			cellpediaView.SetActive(false);
		}

		public void OpenView()
		{
			cellpediaView.SetActive(true);

			if (PetridishButton.selected == null)
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
			else
			{
				selectedCellObject = PetridishButton.selected.cellObject;
			}
			buttonsBar.OnOpen(selectedCellObject);
			microscope.OnOpen(selectedCellObject);
			notepad.OnOpen(selectedCellObject);

			microscopeButtonAnimator.SetTrigger("Opened");
		}


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
			microscopeButton.SetActive(true);
			buttonsBar.ActivateButton(item);

			CellpediaPopup popup = Instantiate(popupPrefab, popupLayout.transform, false).GetComponent<CellpediaPopup>();
			popup.SetInfo(item);
			microscopeButtonAnimator.SetTrigger("NewItem");
			SaveData();
		}

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
		}
	}

}