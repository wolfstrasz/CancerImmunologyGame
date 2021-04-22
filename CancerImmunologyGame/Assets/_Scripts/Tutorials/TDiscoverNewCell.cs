using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.CellpediaUI;

namespace ImmunotherapyGame.Tutorials
{
	public class TDiscoverNewCell : TutorialEvent
	{
		[Header("Attributes")]
		[SerializeField]
		private CellpediaCells cellToDiscover = CellpediaCells.NONE;

		protected override void OnEndEvent()
		{
		}

		protected override void OnStartEvent()
		{
			Cellpedia.Instance.UnlockCellDescription(cellToDiscover);
		}

		protected override bool OnUpdateEvent()
		{
			return true;
		}
	}
}
