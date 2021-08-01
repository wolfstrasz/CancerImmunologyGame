using UnityEngine;
using ImmunotherapyGame.CellpediaSystem;

namespace ImmunotherapyGame.Tutorials
{
	public class TDiscoverNewCell : TutorialEvent
	{
		[Header("Attributes")]
		[SerializeField]
		private CellpediaItemTypes cellToDiscover = CellpediaItemTypes.NONE;

		protected override void OnStartEvent()
		{
			Cellpedia.Instance.UnlockCellDescription(cellToDiscover);
		}
	}
}
