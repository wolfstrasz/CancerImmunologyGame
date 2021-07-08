using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.LevelTasks;

namespace ImmunotherapyGame.Tutorials
{
	public class TCreateLevelTask : TutorialEvent
	{
		[SerializeField] private LevelTaskObject levelTaskObject;
		[SerializeField] private string title;
		[SerializeField] private int count;
		[SerializeField] private int awardPoints;

		protected override void OnEndEvent()
		{
		}

		protected override void OnStartEvent()
		{
			LevelTaskSystem.Instance.CreateTask(levelTaskObject, title, count, awardPoints);
		}

		protected override bool OnUpdateEvent()
		{
			return true;
		}
	}
}
