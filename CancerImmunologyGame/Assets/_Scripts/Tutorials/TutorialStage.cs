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
				currentEvent = events[event_index];
				++event_index;
				currentEvent.owner = this;
				currentEvent.Start();
			}
			else
			{
				StageFinished();
			}
		}

		public void OnEventFinished()
		{
			Debug.Log(gameObject.name + ": On Event Finished");

			if (event_index < events.Count)
			{
				currentEvent = Instantiate(events[event_index], this.transform, true).GetComponent<TutorialEvent>();
				++event_index;
				currentEvent.owner = this;
				currentEvent.Start();
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
