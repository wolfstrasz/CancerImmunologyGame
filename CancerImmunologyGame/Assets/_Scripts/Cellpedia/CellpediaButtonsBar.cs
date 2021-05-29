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
			List<CellpediaObject> cellData = Cellpedia.Instance.data.cellpediaItems;
			petridishButtonList = new List<PetridishButton>(cellData.Count);
			petridishButtons = new Dictionary<CellpediaObject, PetridishButton>(cellData.Count);

			// Create and init buttons
			for (int i = 0; i < cellData.Count; ++i)
			{
				PetridishButton button = Instantiate(cellbarButtonPrefab, buttonsLayout).GetComponent<PetridishButton>();
				petridishButtons.Add(cellData[i], button);
				petridishButtonList.Add(button);
				button.Initialise(cellData[i], i);
				button.gameObject.name = "PetridishButton: " + cellData[i].cellname;
				// Setting it multile times so that there is no need to check for null multiple times in OnOpen()
				// But we always need selected to be != null for the first time OnOpen() is called
				//PetridishButton.selected = button;

				if (cellData[i].isUnlocked)
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
