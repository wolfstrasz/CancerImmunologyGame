using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.LevelTasks;

namespace ImmunotherapyGame.Tutorials
{
	public class TCreateLevelTask : TutorialEvent
	{
		[SerializeField] private LevelTask levelTask;
	

		protected override void OnEndEvent()
		{
		}

		protected override void OnStartEvent()
		{
			LevelTaskSystem.Instance.CreateTask(levelTask);
		}

		protected override bool OnUpdateEvent()
		{
			return true;
		}
	}
}
