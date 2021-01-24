using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CellpediaUI
{
	public class Cellpedia : Singleton<Cellpedia>
	{
		[Header("Link")]
		[SerializeField]
		private List<Petridish> petridishes = new List<Petridish>();
		[SerializeField]
		private List<CellDescriptionLink> cellDescriptions = new List<CellDescriptionLink>();
		[SerializeField]
		TMP_Text cellDescription = null;
		[SerializeField]
		TMP_Text cellName = null;
		[SerializeField]
		private Animator buttonAnimator = null;


		[Header("Attributes")]
		[SerializeField]
		private float time_between_shifts = 2.0f;
		private const float time_for_button_rotation = 2.0f;

		[Header("Debugging")]
		[SerializeField]
		private List<CellDescription> unlockedCellDescriptions = new List<CellDescription>();
		[SerializeField]
		private Dictionary<CellpediaCells, CellDescription> cellpediaDescriptions = new Dictionary<CellpediaCells, CellDescription>();
		[SerializeField]
		private int cdIndex = 0;
		[SerializeField]
		private int dishIndex = 0;

		public bool IsCellpediaOpened => gameObject.activeSelf;

		void Start()
		{
			//Initialise();
		}


		public void Initialise()
		{
			buttonAnimator.speed = buttonAnimator.speed * time_for_button_rotation / time_between_shifts;
			Petridish.Timetopass = time_between_shifts;
			foreach (CellDescriptionLink cdl in cellDescriptions)
			{
				cellpediaDescriptions.Add(cdl.cc, cdl.cd);
			}

			gameObject.SetActive(false);
		}

		public void Close()
		{
			gameObject.SetActive(false);
		}

		public void Open()
		{
			gameObject.SetActive(true);
			CellDescription cd = unlockedCellDescriptions[cdIndex];
			petridishes[dishIndex].SetVisual(cd);
			cellDescription.text = cd.description;
			cellName.text = cd.cellname;
		}

		public void UnlockCellDescription(CellpediaCells cc)
		{
			if (cc == CellpediaCells.NONE) return;

			unlockedCellDescriptions.Add(cellpediaDescriptions[cc]);
			Player.PlayerUI.Instance.StartGlow();
			// Make button glow!
		}

		public void NextPetridish()
		{
			Debug.Log("Button Clicked");

			//if (unlockedCellDescriptions.Count <= 1) return;

			if (petridishes[0].isShifting || petridishes[1].isShifting) return;

			Debug.Log("Button Click Executed");
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


		[System.Serializable]
		public struct CellDescriptionLink
		{
			public CellDescription cd;
			public CellpediaCells cc;
		}
	}
}