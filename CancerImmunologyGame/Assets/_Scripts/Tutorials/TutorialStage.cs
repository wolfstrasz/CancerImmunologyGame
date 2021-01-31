using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorials
{
	public class TutorialStage : MonoBehaviour
	{
		public List<TutorialEvent> events = new List<TutorialEvent>();
		public TutorialEvent currentEvent = null;
		private int event_index = 0;

		public void StartStage()
		{
			Debug.Log(gameObject.name + ": Start Stage");
			if (event_index < events.Count)
			{
				NextEvent();
			}
			else
			{
				StageFinished();
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

		public void OnEventFinished()
		{
			currentEvent.gameObject.SetActive(false);
			Debug.Log(gameObject.name + ": On Event Finished");

			if (event_index < events.Count)
			{
				NextEvent();
			}
			else
			{
				StageFinished();
			}
		}

		public void StageFinished()
		{
			Debug.Log(gameObject.name + ": Stage finsihed");
			TutorialManager.Instance.OnStageFinished(this);
		}


	}
}
