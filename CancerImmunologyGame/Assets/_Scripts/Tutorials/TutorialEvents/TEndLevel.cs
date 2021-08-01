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
			LevelManager.Instance.OnLevelComplete();
		}

    }
}
