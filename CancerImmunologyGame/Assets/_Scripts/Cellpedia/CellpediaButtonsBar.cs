using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ImmunotherapyGame.Core;
using ImmunotherapyGame.UI;

namespace ImmunotherapyGame.CellpediaSystem
{
    public class CellpediaButtonsBar : MonoBehaviour
    {
		[SerializeField]
		private CellpediaData cellData = null;
		[SerializeField]
		private GameObject cellbarButtonPrefab = null;
		[SerializeField]
		private Transform buttonsLayout = null;
		[ReadOnly]
        private Dictionary<CellpediaCellDescription, PetridishButton> petridishButtons = null;
		[ReadOnly]
		internal List<PetridishButton> petridishButtonList = null;

		internal void Initialise()
		{
			List<CellpediaCellDescription> cellDataList = cellData.cellpediaItems;
			petridishButtonList = new List<PetridishButton>(cellDataList.Count);
			petridishButtons = new Dictionary<CellpediaCellDescription, PetridishButton>(cellDataList.Count);

			// Create and init buttons
			for (int i = 0; i < cellDataList.Count; ++i)
			{
				PetridishButton button = Instantiate(cellbarButtonPrefab, buttonsLayout).GetComponent<PetridishButton>();
				petridishButtons.Add(cellDataList[i], button);
				petridishButtonList.Add(button);
				button.Initialise(cellDataList[i], i);
				button.gameObject.name = "PetridishButton: " + cellDataList[i].cellName;
				// Setting it multile times so that there is no need to check for null multiple times in OnOpen()
				// But we always need selected to be != null for the first time OnOpen() is called
				//PetridishButton.selected = button;

				if (cellDataList[i].IsUnlocked)
					button.Activate();
				else button.Deactivate();
			}

			PetridishButton.allPetridishButtons = petridishButtonList;
		}

		internal void OnOpen(CellpediaCellDescription cellDescription)
		{
			if (cellDescription == null) return;
			PetridishButton.lastSubmitted = petridishButtons[cellDescription];
			PetridishButton.lastSubmitted.SubmitCellData();
		}

        internal void ActivateButton(CellpediaCellDescription cellDescription)
		{
			petridishButtons[cellDescription].Activate();
		}
    }
}
