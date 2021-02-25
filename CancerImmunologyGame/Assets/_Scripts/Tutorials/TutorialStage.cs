using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorials
{
	public class TutorialStage : MonoBehaviour
	{
		[SerializeField]
		private List<TutorialEvent> events = new List<TutorialEvent>();
		[SerializeField]
		private TutorialEvent currentEvent = null;
		private int event_index = 0;
		internal bool isFinished = false;

	
		internal void OnUpdate()
		{
			currentEvent.OnUpdate();
		}


		internal void InitialiseStage()
		{
			//events = new List<TutorialEvent>();
			//events.AddRange(gameObject.GetComponentsInChildren<TutorialEvent>());
			if (event_index < events.Count)
			{
				NextEvent();
			}
			else
			{
				isFinished = true;
			}
		}

		private void NextEvent()
		{
			currentEvent = events[event_index];
			currentEvent.gameObject.SetActive(true);
			++event_index;
			currentEvent.owner = this;
			currentEvent.StartEvent();

		}

		internal void OnEventFinished()
		{
			currentEvent.gameObject.SetActive(false);
			Debug.Log(gameObject.name + ": On Event Finished");

			if (event_index < events.Count)
			{
				NextEvent();
			}
			else
			{
				isFinished = true;
			}
		}

	}
}
