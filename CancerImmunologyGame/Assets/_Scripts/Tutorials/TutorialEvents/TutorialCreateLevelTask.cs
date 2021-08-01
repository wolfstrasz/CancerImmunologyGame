using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.LevelTasks;

namespace ImmunotherapyGame.Tutorials
{
	public class TutorialCreateLevelTask : TutorialEvent
	{
		[SerializeField] private LevelTask levelTask;

		protected override void OnStartEvent()
		{
			LevelTaskSystem.Instance.CreateTask(levelTask);
		}

	}
}
