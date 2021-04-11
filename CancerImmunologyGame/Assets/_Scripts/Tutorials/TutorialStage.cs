using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ImmunotherapyGame.Tutorials
{
	public class TutorialStage : MonoBehaviour
	{
		[SerializeField]
		private List<TutorialEvent> events = null;
		[SerializeField]
		private TutorialEvent currentEvent = null;
		[SerializeField]
		private int currentEventIndex = 0;
		[SerializeField]
		private bool isFinished = false;

		internal bool IsFinished { get => isFinished; }

		internal void OnUpdate()
		{
			if (isFinished) return;

			// If event is finished
			if (currentEvent.OnUpdate())
			{
				// Stop previous event
				currentEvent.EndEvent();
				currentEvent.gameObject.SetActive(false);
				Debug.Log(gameObject.name + " event finished: " + currentEvent.name);
				currentEventIndex++;

				// Finish stage if reached last event
				if (currentEventIndex >= events.Count)
				{
					isFinished = true;
					return;
				}

				// Start next event
				currentEvent = events[currentEventIndex];
				currentEvent.gameObject.SetActive(true);    // Debugging
				currentEvent.StartEvent();
			}
		}

		internal void InitialiseStage()
		{
			// Collect events and sort them by priority
			List<TutorialEvent> eventsFound = new List<TutorialEvent>(GetComponentsInChildren<TutorialEvent>(true));
			eventsFound.OrderBy(e => e.order);
			events = eventsFound;

			// Run the first event
			if (events.Count > 0)
			{
				currentEvent = events[currentEventIndex];
				currentEvent.gameObject.SetActive(true);	// Debugging
				currentEvent.StartEvent();
			}

		}

	}
}
