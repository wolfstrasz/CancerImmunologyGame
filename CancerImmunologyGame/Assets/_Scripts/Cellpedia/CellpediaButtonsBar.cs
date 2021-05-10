using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.Core;

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

		internal void Initialise()
		{
			List<CellpediaObject> cellData = Cellpedia.Instance.data.cellpediaItems;
			petridishButtons = new Dictionary<CellpediaObject, PetridishButton>();

			// Create and init buttons
			for (int i = 0; i < cellData.Count; ++i)
			{
				PetridishButton button = Instantiate(cellbarButtonPrefab, buttonsLayout).GetComponent<PetridishButton>();
				petridishButtons.Add(cellData[i], button);
				button.Initialise(cellData[i]);
				// Setting it multile times so that there is no need to check for null multiple times in OnOpen()
				// But we always need selected to be != null for the first time OnOpen() is called
				//PetridishButton.selected = button;

				if (cellData[i].isUnlocked)
					button.Activate();
				else button.Deactivate();
			}

		}

		internal void OnOpen(CellpediaObject cellObject)
		{
			if (cellObject == null) return;
			if (PetridishButton.selected != null)
				PetridishButton.selected.DeselectCell();
			PetridishButton.selected = petridishButtons[cellObject];
			PetridishButton.selected.SelectCell();
		}

        internal void ActivateButton(CellpediaObject cellObject)
		{
			petridishButtons[cellObject].Activate();
		}
    }
}
