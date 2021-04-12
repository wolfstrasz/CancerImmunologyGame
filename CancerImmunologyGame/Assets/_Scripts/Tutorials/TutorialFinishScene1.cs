using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.Tutorials
{
    public class TutorialFinishScene1 : TutorialEvent
    {
		[SerializeField]
		private int build_index = 0;

		protected override void OnEndEvent()
		{
		}

		protected override void OnStartEvent()
		{
			if (build_index == 2)
				UIManager.Instance.OpenWinScenePanel1();
			if (build_index == 3)
			{
				UIManager.Instance.OpenWinScenePanel1();
			}
		}

		protected override bool OnUpdateEvent()
		{
			return true;
		}
    }
}
