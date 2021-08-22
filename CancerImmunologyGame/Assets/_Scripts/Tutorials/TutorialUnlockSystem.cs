using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ImmunotherapyGame.CellpediaSystem;
using ImmunotherapyGame.ImmunotherapyResearchSystem;



namespace ImmunotherapyGame.Tutorials
{

    public class TutorialUnlockSystem : TutorialEvent
    {
        [SerializeField] private SystemItemToUnlock itemType;

		protected override void OnStartEvent()
		{
			if (itemType == SystemItemToUnlock.LOGS)
			{
				TutorialManager.Instance.UnlockFeature();
			}
			else if (itemType == SystemItemToUnlock.CELLPEDIA)
			{
				Cellpedia.Instance.UnlockFeature();
			} 
			else if (itemType == SystemItemToUnlock.RESEARCH)
			{
				ImmunotherapyResearch.Instance.UnlockFeature();
			}
		}

		public enum SystemItemToUnlock { CELLPEDIA, LOGS, RESEARCH }
    }




}
