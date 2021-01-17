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
		[Header("Attributes")]
		private float time_between_shifts = 2.0f;

		[Header("Debugging")]
		[SerializeField]
		private List<CellDescription> unlockedCellDescriptions = new List<CellDescription>();
		[SerializeField]
		private Dictionary<CellpediaCells, CellDescription> cellpediaDescriptions = new Dictionary<CellpediaCells, CellDescription>();
		[SerializeField]
		private int cdIndex = 0;
		[SerializeField]
		private int dishIndex = 0;
		[SerializeField]
		bool rotatorLocked = false;

		void Start()
		{
			Initialise();
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Z))
			{
				UnlockCellDescription(CellpediaCells.DENDRITIC);
			}
			if (Input.GetKeyDown(KeyCode.X))
			{
				UnlockCellDescription(CellpediaCells.REGULATORY);
			}
			if (Input.GetKeyDown(KeyCode.C))
			{
				UnlockCellDescription(CellpediaCells.CANCER);
			}
			if (Input.GetKeyDown(KeyCode.V))
			{
				UnlockCellDescription(CellpediaCells.THELPER);
			}

		}

		public void Initialise()
		{
			Petridish.Timetopass = time_between_shifts;
			foreach (CellDescriptionLink cdl in cellDescriptions)
			{
				cellpediaDescriptions.Add(cdl.cc, cdl.cd);
			}

			UnlockCellDescription(CellpediaCells.TKILLER);
			petridishes[0].SetVisual(unlockedCellDescriptions[0]);
		}

		public void Close()
		{
			gameObject.SetActive(false);
		}

		public void Open()
		{
			gameObject.SetActive(true);
		}

		public void UnlockCellDescription(CellpediaCells cc)
		{
			unlockedCellDescriptions.Add(cellpediaDescriptions[cc]);

			// Make button glow!
		}

		public void NextPetridish()
		{
			if (unlockedCellDescriptions.Count <= 1) return;

			cdIndex++;
			cdIndex %= unlockedCellDescriptions.Count;
			dishIndex ^= 1;

			if (!rotatorLocked)
			{
				rotatorLocked = true;
				var cd = unlockedCellDescriptions[cdIndex];
				petridishes[dishIndex].SetVisual(cd);
				petridishes[0].ShiftLeft();
				petridishes[1].ShiftLeft();
				cellDescription.text = cd.description;
				cellName.text = cd.cellname;

				StartCoroutine(WaitForRotator());
			}
		}

		private IEnumerator WaitForRotator()
		{
			yield return new WaitForSeconds(time_between_shifts);
			rotatorLocked = false;
		}

		[System.Serializable]
		public struct CellDescriptionLink
		{
			public CellDescription cd;
			public CellpediaCells cc;
		}
	}
}