using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using ImmunotherapyGame.LevelManagement;


namespace ImmunotherapyGame.Tutorials
{
    public class TEndLevel : TutorialEvent
    {
		protected override void OnEndEvent()
		{
			LevelManager.Instance.OnLevelComplete(SceneManager.GetActiveScene().buildIndex);

			for(int i = 0; i < GlobalGameData.dataManagers.Count; ++i)
			{
				GlobalGameData.dataManagers[i].SaveData();
			}


		}

		protected override void OnStartEvent()
		{
		}

		protected override bool OnUpdateEvent()
		{
			return true;
		}

    }
}
