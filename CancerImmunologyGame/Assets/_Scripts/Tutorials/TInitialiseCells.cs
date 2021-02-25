using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cells;
namespace Tutorials
{
	public class TInitialiseCells : TutorialEvent
	{
		[SerializeField]
		CellpediaCells cellsToInitialise = CellpediaCells.NONE;

		protected override void OnEndEvent()
		{
		}

		protected override void OnStartEvent()
		{
			if (cellsToInitialise == CellpediaCells.NONE) return;

			if (cellsToInitialise == CellpediaCells.THELPER)
			{
				var helperCells = FindObjectsOfType<HelperTCell>();

				foreach (HelperTCell cell in helperCells)
				{
					cell.Activate();
				}
			}

			if (cellsToInitialise == CellpediaCells.REGULATORY)
			{
				var regulatoryCells = FindObjectsOfType<RegulatoryCell>();

				foreach (RegulatoryCell cell in regulatoryCells)
				{
					cell.gameObject.SetActive(true);
				}
			}
		}

		protected override bool OnUpdateEvent()
		{
			return true;
		}

		
	}
}