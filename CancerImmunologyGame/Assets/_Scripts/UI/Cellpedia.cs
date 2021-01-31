using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CellpediaUI
{
	public class Cellpedia : Singleton<Cellpedia>
	{
		[Header("Cellpedia View")]
		[SerializeField]
		private GameObject cellpediaView = null;
		[SerializeField]
		private List<CellDescriptionLink> cellDescriptions = new List<CellDescriptionLink>();
		[SerializeField]
		private List<Petridish> petridishes = new List<Petridish>();
		[SerializeField]
		private Animator buttonAnimator = null;

		[SerializeField]
		TMP_Text cellDescription = null;
		[SerializeField]
		TMP_Text cellName = null;
		[SerializeField]
		private float time_between_shifts = 2.0f;
		private const float time_for_button_rotation = 2.0f;

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
		private List<CellDescription> unlockedCellDescriptions = new List<CellDescription>();
		[SerializeField]
		private Dictionary<CellpediaCells, CellDescription> cellpediaDescriptions = new Dictionary<CellpediaCells, CellDescription>();
		[SerializeField]
		private int cdIndex = 0;
		[SerializeField]
		private int dishIndex = 0;


		internal Vector3 MicroscopeButtonPosition => microscopeButton.transform.position;
		internal Transform PopupLayout => popupLayout.transform;

		// Used by tutorials
		public bool IsCellpediaOpened => cellpediaView.gameObject.activeSelf;


		public void Initialise()
		{
			buttonAnimator.speed = buttonAnimator.speed * time_for_button_rotation / time_between_shifts;
			Petridish.Timetopass = time_between_shifts;
			foreach (CellDescriptionLink cdl in cellDescriptions)
			{
				cellpediaDescriptions.Add(cdl.cc, cdl.cd);
			}
		}

		// UI Button callbacks
		public void CloseView()
		{
			cellpediaView.SetActive(false);
		}

		public void OpenView()
		{
			cellpediaView.SetActive(true);
			CellDescription cd = unlockedCellDescriptions[cdIndex];
			petridishes[dishIndex].SetVisual(cd);
			cellDescription.text = cd.description;
			cellName.text = cd.cellname;
			microscopeAnimator.SetTrigger("Opened");
		}

		public void NextPetridish()
		{

			if (petridishes[0].isShifting || petridishes[1].isShifting) return;

			buttonAnimator.Play("Rotate");
			cdIndex++;
			cdIndex %= unlockedCellDescriptions.Count;
			dishIndex ^= 1;

			CellDescription cd = unlockedCellDescriptions[cdIndex];
			petridishes[dishIndex].SetVisual(cd);
			cellDescription.text = cd.description;
			cellName.text = cd.cellname;
			petridishes[0].ShiftLeft();
			petridishes[1].ShiftLeft();

		}

		public void UnlockCellDescription(CellpediaCells cc)
		{
			if (cc == CellpediaCells.NONE) return;
			microscopeButton.SetActive(true);
			unlockedCellDescriptions.Add(cellpediaDescriptions[cc]);
			CellpediaPopup popup = Instantiate(popupPrefab, popupLayout.transform, false).GetComponent<CellpediaPopup>();
			popup.SetInfo(cellpediaDescriptions[cc]);
			microscopeAnimator.SetTrigger("NewItem");

		}

		[System.Serializable]
		public struct CellDescriptionLink
		{
			public CellDescription cd;
			public CellpediaCells cc;
		}
	}
}