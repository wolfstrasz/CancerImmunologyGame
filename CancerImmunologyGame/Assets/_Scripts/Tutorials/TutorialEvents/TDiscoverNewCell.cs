using UnityEngine;
using ImmunotherapyGame.CellpediaSystem;

namespace ImmunotherapyGame.Tutorials
{
	public class TDiscoverNewCell : TutorialEvent
	{
		[Header("Attributes")]
		[SerializeField] [Expandable] private CellpediaCellDescription cellToDiscover;
		[SerializeField] private bool unlockNote1;
		[SerializeField] private bool unlockNote2;
		[SerializeField] private bool unlockNote3;

		protected override void OnStartEvent()
		{
			if (unlockNote1)
			{
				cellToDiscover.Note1IsUnlocked = true;
			}
			if (unlockNote2)
			{
				cellToDiscover.Note2IsUnlocked = true;
			}
			if (unlockNote3)
			{
				cellToDiscover.Note3IsUnlocked = true;
			}

			if (!cellToDiscover.IsUnlocked)
			{
				Cellpedia.Instance.UnlockCellDescription(cellToDiscover);
			}
		}
	}
}
