using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.Core;

namespace ImmunotherapyGame.CellpediaSystem
{
	public class Cellpedia : Singleton<Cellpedia>
	{
		[Header("Cellpedia View Data")]
		[SerializeField]
		private GameObject cellpediaView = null;
		[SerializeField]
		private CellpediaNotepad notepad = null;
		[SerializeField]
		private List<Petridish> petridishes = new List<Petridish>();
		[SerializeField]
		private GameObject leftBtn = null;
		[SerializeField]
		private GameObject rightBtn = null;

		[SerializeField]
		private List<CellDescriptionLink> cellDescriptions = new List<CellDescriptionLink>();
		[Header ("Game UI")]
		[SerializeField]
		private GameObject microscopeButton = null;
		[SerializeField]
		private Animator microscopeAnimator = null;

		[Header ("Popups")]
		[SerializeField]
		private GameObject popupLayout = null;
		[SerializeField]
		private GameObject popupPrefab = null;

		[Header("Debugging (Read Only")]
		[SerializeField]
		private CellDescription currentCD = null;
		[SerializeField]
		private int dishIndex = 0;

		[SerializeField]
		private int cellsFound = 0;

		// Used by Cellpedia popups
		internal Transform PopupLayout => popupLayout.transform;
		internal Vector3 MicroscopeButtonPosition => microscopeButton.transform.position;

		// Used by tutorials
		public bool IsCellpediaOpened => cellpediaView.gameObject.activeSelf;


		public void Initialise()
		{
			leftBtn.SetActive(false);
			rightBtn.SetActive(false);
			foreach (CellDescriptionLink cdl in cellDescriptions)
			{
				cdl.button.Initialise(cdl.description.sprite);
				cdl.button.Deactivate();
				PetridishButton.selected = null;
				if (dishIndex != 0)
				{
					petridishes[0].Reset();
					petridishes[1].Reset();
					dishIndex = 0;
				}
				currentCD = null;
				microscopeButton.SetActive(cellsFound > 0);

			}
		}

		// UI Button callbacks
		public void CloseView()
		{
			cellpediaView.SetActive(false);
			petridishes[0].SkipAnimation();
			petridishes[1].SkipAnimation();
		}

		public void OpenView()
		{
			cellpediaView.SetActive(true);

			if (PetridishButton.selected == null)
			{
				currentCD = cellDescriptions[0].description;
				PetridishButton.selected = cellDescriptions[0].button;
				PetridishButton.selected.Select();
				petridishes[dishIndex].SetVisual(currentCD);
				notepad.SetVisual(currentCD);
			}
			petridishes[dishIndex].SetVisual(currentCD);
			notepad.SetVisual(currentCD);


			microscopeAnimator.SetTrigger("Opened");
		}

		internal bool NextPetridish(PetridishButton buttonClicked)
		{

			if (petridishes[0].isShifting || petridishes[1].isShifting) return false;
			dishIndex ^= 1;

			for (int i = 0; i < cellDescriptions.Count; ++i)
			{
				CellDescriptionLink cdLink = cellDescriptions[i];
				if (cdLink.button == buttonClicked)
				{
					currentCD = cdLink.description;
					petridishes[dishIndex].SetVisual(currentCD);
					notepad.SetVisual(currentCD);

					petridishes[0].ShiftLeft();
					petridishes[1].ShiftLeft();
					continue;
				}
			}
			return true;
		}

		public void UnlockCellDescription(CellpediaCells cc)
		{
			if (cc == CellpediaCells.NONE) return;
			
			for (int i = 0; i< cellDescriptions.Count; ++i)
			{
				CellDescriptionLink cdLink = cellDescriptions[i];
				if (cdLink.cell == cc)
				{
					microscopeButton.SetActive(true);
					cdLink.button.Activate();
					CellpediaPopup popup = Instantiate(popupPrefab, popupLayout.transform, false).GetComponent<CellpediaPopup>();
					popup.SetInfo(cdLink.description);
					microscopeAnimator.SetTrigger("NewItem");
					cellsFound++;
					continue;
				}
			}
		}


		
		[System.Serializable]
		public struct CellDescriptionLink
		{
			public CellDescription description;
			public CellpediaCells cell;
			public PetridishButton button;
		}
	}

	public enum CellpediaCells { NONE, TKILLER, THELPER, DENDRITIC, REGULATORY, CANCER, CAF }
}