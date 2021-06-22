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
        private Dictionary<CellpediaObject, PetridishButton> petridishButtons = null;
		[ReadOnly]
		internal List<PetridishButton> petridishButtonList = null;
		[SerializeField]
		private PetridishButton initialButton = null;

		internal void Initialise()
		{
			List<CellpediaObject> cellDataList = cellData.cellpediaItems;
			petridishButtonList = new List<PetridishButton>(cellDataList.Count);
			petridishButtons = new Dictionary<CellpediaObject, PetridishButton>(cellDataList.Count);

			// Create and init buttons
			for (int i = 0; i < cellDataList.Count; ++i)
			{
				PetridishButton button = Instantiate(cellbarButtonPrefab, buttonsLayout).GetComponent<PetridishButton>();
				petridishButtons.Add(cellDataList[i], button);
				petridishButtonList.Add(button);
				button.Initialise(cellDataList[i], i);
				button.gameObject.name = "PetridishButton: " + cellDataList[i].cellname;
				// Setting it multile times so that there is no need to check for null multiple times in OnOpen()
				// But we always need selected to be != null for the first time OnOpen() is called
				//PetridishButton.selected = button;

				if (cellDataList[i].isUnlocked)
					button.Activate();
				else button.Deactivate();
			}

			PetridishButton.allPetridishButtons = petridishButtonList;
		}

		internal void OnOpen(CellpediaObject cellObject)
		{
			if (cellObject == null) return;
			PetridishButton.lastSubmitted = petridishButtons[cellObject];
			PetridishButton.lastSubmitted.SubmitCellData();
		}

        internal void ActivateButton(CellpediaObject cellObject)
		{
			petridishButtons[cellObject].Activate();
		}
    }
}
